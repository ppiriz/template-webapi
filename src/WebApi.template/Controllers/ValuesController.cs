using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.Description;
using DevOpsFlex.Core;
using DevOpsFlex.Telemetry;
using Microsoft.AspNetCore.Mvc;
using WebApi.template.Models;

namespace WebApi.template.Controllers
{
    /// <summary>
    /// Demo values controller, based on the values controller in the default VS template.
    /// </summary>
    /// <remarks>
    /// review http://www.ietf.org/assignments/http-status-codes/http-status-codes.xml for all status codes
    /// </remarks>
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private const int ExistingRecordsRange = 100;
        private const int RestrictedRecordsRange = 10;

        private readonly BasicDependency _basicDependency;
        private readonly IBigBrother _bb;

        /// <summary>
        /// </summary>
        /// <param name="basicDependency"></param>
        /// <param name="bb"></param>
        public ValuesController(BasicDependency basicDependency, IBigBrother bb)
        {
            _basicDependency = basicDependency;
            _bb = bb;
#if DEBUG
            // Don't perform these checks on Release code, as these are only development time errors
            if (basicDependency == null) throw new ArgumentNullException(nameof(basicDependency));
            if (bb == null) throw new ArgumentNullException(nameof(bb));
#endif
        }

        /// <summary>
        /// returns a collection of values
        /// </summary>
        /// <response code="200">collection of values</response>
        [HttpGet]
        [ResponseType(typeof(string[]))]
        public IEnumerable<string> Get()
        {
            return new[] { $"{_basicDependency.ApplicationName} - value1", $"{_basicDependency.ApplicationName} - value2" };
        }

        /// <summary>
        /// returns a specific value with the specified id
        /// </summary>
        /// <param name="id">id of value to return</param>
        /// <returns>a single value</returns>
        /// <response code="200">record found - body contains data</response>
        /// <response code="404">record does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(void))]
        public IActionResult Get(int id)
        {
            if (id < ExistingRecordsRange)
            {
                return Ok($"{_basicDependency.ApplicationName} - value{id}");
            }

            return NotFound();
        }

        /// <summary>
        /// adds a record type to the system
        /// </summary>
        /// <param name="value">new record to add</param>
        /// <response code="201">resource created</response>
        /// <response code="400">Request is not valid</response>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created,Type=typeof(Uri))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(void))]
        public IActionResult Post([FromBody]Record value)
        {
            if (string.IsNullOrWhiteSpace(value.Name) || string.IsNullOrWhiteSpace(value.Address)) return BadRequest();
            
            var id = new Random().Next(ExistingRecordsRange + 1, 1000);
            _bb.Publish(new ValueAddedEvent(id));

            return Created($"/api/values/{id}", id);
        }

        /// <summary>
        /// modifies the existing resource
        /// </summary>
        /// <param name="id">id of resouce</param>
        /// <param name="value">Data to change</param>
        /// <returns></returns>
        /// <response code="204">resource modified</response>
        /// <response code="403">not allowed to modify the resource</response>
        /// <response code="404">resource does not exist</response>
        [ProducesResponseType((int)HttpStatusCode.Forbidden, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [HttpPut("{id}")]
        [ResponseType( typeof(int))]
        public IActionResult Put(int id, [FromBody]Record value)
        {
            if (id < RestrictedRecordsRange) return Forbid();
            if (id < ExistingRecordsRange) return NoContent();

            return NotFound();
        }

        /// <summary>
        /// removes a record from the system
        /// </summary>
        /// <param name="id">id of resource to remove</param>
        /// <response code="204">resource removed</response>
        /// <response code="403">not allowed to remove the resource</response>
        /// <response code="404">resource does not exist</response>
        [ProducesResponseType((int)HttpStatusCode.Forbidden, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NoContent, Type = typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id < RestrictedRecordsRange) return Forbid();
            if (id < ExistingRecordsRange) return NoContent();

            return NotFound();
        }
    }

    /// <summary>
    /// Basic dependency demo class.
    /// </summary>
    public class BasicDependency
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BasicDependency"/>
        /// </summary>
        public BasicDependency()
        {
            ApplicationName = "webApi.template";
        }

        /// <summary>
        /// Gets a demo <see cref="string"/> property.
        /// </summary>
        public virtual string ApplicationName { get; }
    }

    /// <summary>
    /// Telemetry demo event for PUTs on the <see cref="ValuesController"/>.
    /// </summary>
    public class ValueAddedEvent : BbTelemetryEvent
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValueAddedEvent"/>.
        /// </summary>
        /// <param name="id">The Id of the value added.</param>
        public ValueAddedEvent(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the Id of the value added.
        /// </summary>
        public int Id { get; }
    }
}
