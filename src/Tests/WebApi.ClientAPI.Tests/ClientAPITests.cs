using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpsFlex.Tests.Core;
using Xunit;
//using FluentAssertions;
using Newtonsoft.Json;
using webApi.ClientAPI;

namespace webApi.template.tests.Integration
{
    public class ClientAPITests
    {
        private readonly WebApitemplate _apiv1;

        public ClientAPITests()
        {
            _apiv1 = new WebApitemplate(new Uri("http://localhost:63300"));
        }

        [Fact, IsIntegration]
        public void ApiValuesGet()
        {
            // Arrange
            // Act
            var result = _apiv1.ApiValuesGet();

            // Assert
            Assert.True(result.Count() > 1);
        }

        [Fact, IsIntegration]
        public async Task ApiValuesByNameGetWithHttpMessagesAsync()
        {
            var httpResult = await _apiv1.ApiValuesByIdGetWithHttpMessagesAsync(50);
            Assert.Equal(httpResult.Response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.NotNull(httpResult.Body);
            Assert.Contains(httpResult.Body, "- value50");
        }

        [Fact, IsIntegration]
        public async Task ApiValuesByNameGetWithHttpMessagesAsync2()
        {
            var httpResult = await _apiv1.ApiValuesByIdGetWithHttpMessagesAsync(200);
            Assert.Equal(httpResult.Response.StatusCode, System.Net.HttpStatusCode.NotFound);
            Assert.Null(httpResult.Body);
        }
    }
}