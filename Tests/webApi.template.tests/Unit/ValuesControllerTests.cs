﻿using System;
using DevOpsFlex.Tests.Core;
using webApi.template.Controllers;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace webApi.template.tests
{
    public class ValuesControllerTests
    {
        public class Get
        {
            private const string AppName = "foo";
            private readonly ValuesController _controller;
            private readonly Mock<IBasicDependency> _basicDependencyMoq;
            public Get()
            {
                _basicDependencyMoq = new Mock<IBasicDependency>();
                _basicDependencyMoq.SetupGet(p => p.ApplicationName).Returns(AppName);

                _controller = new ValuesController(_basicDependencyMoq.Object);
            }

            [Fact, IsUnit]
            public void WithNoParameter_ReturnsEnumerableResult()
            {
                // Arrange
                // Act
                var result = _controller.Get();

                // Assert
                result.Should().NotBeNull().And.ContainInOrder($"{AppName} - value1", $"{AppName} - value2");
            }

            [Fact, IsUnit]
            public void WithParameter_ReturnsOkResultWithSpecificResource()
            {
                // Arrange
                // Act
                var result = _controller.Get(1);

                // Assert
                result.As<OkObjectResult>().Value.Should().Be("foo - value1");
            }

            [Fact, IsUnit]
            public void ChangingApplicationNameDependency_ResultOutputIsCorrect()
            {
                // Arrange
                _basicDependencyMoq.SetupGet(p => p.ApplicationName).Returns("bar"); // override default setup

                // Act
                var result = _controller.Get(1);

                // Assert
                result.As<OkObjectResult>().Value.Should().Be("bar - value1");
            }

            

            [Fact, IsUnit]
            public void WithOutOfRangeParameter_RetrunsNotFoundResult()
            {
                // Arrange
                // Act
                var result = _controller.Get(300);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
            }
        }
    }
}