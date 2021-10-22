using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions.Extensions.Queries;
using QueryFramework.Abstractions.Queries;
using Xunit;

namespace QueryFramework.Abstractions.Tests.Extensions.Queries
{
    [ExcludeFromCodeCoverage]
    public class SingleEntityQueryExtensions
    {
        [Fact]
        public void Validate_Validates_Instance_When_ValidateFieldNames_Is_True()
        {
            // Arrange
            var sut = new SingleEntityQueryMock
            {
                ValidationResultValue = new[] { new ValidationResult("kaboom") }
            };

            // Act
            sut.Invoking(x => x.Validate(true))
               .Should().Throw<ValidationException>()
               .And.Message.Should().Be("kaboom");
        }

        [Fact]
        public void Validate_Does_Not_Validate_Instance_When_ValidateFieldNames_Is_False()
        {
            // Arrange
            var sut = new SingleEntityQueryMock
            {
                ValidationResultValue = new[] { new ValidationResult("kaboom") }
            };

            // Act
            sut.Invoking(x => x.Validate(false))
               .Should().NotThrow<ValidationException>();
        }

        [Fact]
        public void GetTableName_Returns_DataObjectName_When_Available_And_Filled()
        {
            // Arrange
            var sut = new SingleEntityQueryMock
            {
                DataObjectName = "custom"
            };

            // Act
            var actual = sut.GetTableName("default");

            // Assert
            actual.Should().Be("custom");
        }

        [Fact]
        public void GetTableName_Returns_TableName_When_DataObjectName_Is_Available_And_Not_Filled()
        {
            // Arrange
            var sut = new SingleEntityQueryMock
            {
                DataObjectName = string.Empty
            };

            // Act
            var actual = sut.GetTableName("default");

            // Assert
            actual.Should().Be("default");
        }

        [Fact]
        public void GetTableName_Returns_TableName_When_DataObjectName_Is_Not_Available()
        {
            // Arrange
            var sut = new Mock<ISingleEntityQuery>();

            // Act
            var actual = sut.Object.GetTableName("default");

            // Assert
            actual.Should().Be("default");
        }

        [ExcludeFromCodeCoverage]
        private class SingleEntityQueryMock : IDataObjectNameQuery, IValidatableObject
        {
            public int? Limit { get; set; }

            public int? Offset { get; set; }

            public IReadOnlyCollection<IQueryCondition> Conditions { get; set; }

            public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

            public string DataObjectName { get; set; }

            public IEnumerable<ValidationResult> ValidationResultValue { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                return ValidationResultValue;
            }
        }
    }
}
