using DevOpsFlex.Telemetry;
using DevOpsFlex.Tests.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.template.Controllers;
using Xunit;

// ReSharper disable once CheckNamespace
public class ValuesControllerTests
{
    public class Get
    {
        private readonly string _appName = Lorem.GetWord();
        private readonly ValuesController _controller;
        private readonly Mock<BasicDependency> _basicDependencyMoq;

        public Get()
        {
            _basicDependencyMoq = new Mock<BasicDependency>();
            _basicDependencyMoq.SetupGet(p => p.ApplicationName).Returns(_appName);

            _controller = new ValuesController(_basicDependencyMoq.Object, new Mock<IBigBrother>().Object);
        }

        [Fact, IsUnit]
        public void WithNoParameter_ReturnsEnumerableResult()
        {
            var result = _controller.Get();

            result.Should().NotBeNull().And.ContainInOrder($"{_appName} - value1", $"{_appName} - value2");
        }

        [Fact, IsUnit]
        public void WithParameter_ReturnsOkResultWithSpecificResource()
        {
            var result = _controller.Get(1);

            result.As<OkObjectResult>().Value.Should().Be($"{_appName} - value1");
        }

        [Fact, IsUnit]
        public void ChangingApplicationNameDependency_ResultOutputIsCorrect()
        {
            var newName = Lorem.GetWord();
            _basicDependencyMoq.SetupGet(p => p.ApplicationName).Returns(newName); // override default setup

            var result = _controller.Get(1);

            result.As<OkObjectResult>().Value.Should().Be($"{newName} - value1");
        }

        [Fact, IsUnit]
        public void WithOutOfRangeParameter_RetrunsNotFoundResult()
        {
            var result = _controller.Get(300);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
