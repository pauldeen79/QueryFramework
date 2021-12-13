using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;
using Xunit;

namespace QueryFramework.Core.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class QueryExpressionExtensionsTests
    {
        [Fact]
        public void Can_Use_With_ExtensionMethod_To_Modify_QueryExpression()
        {
            // Arrange
            var function = new Mock<IQueryExpressionFunction>().Object;
            var sut = new QueryExpression("field", function);
            var newFunction = new Mock<IQueryExpressionFunction>().Object;

            // Act
            var actual = sut.With("newname", newFunction);

            // Assert
            actual.Function.Should().BeSameAs(newFunction);
            actual.FieldName.Should().Be("newname");
        }

        [Fact]
        public void Can_Use_With_ExtensionMethod_With_DefaultValues_To_Create_Instance_With_Same_Values()
        {
            // Arrange
            var function = new Mock<IQueryExpressionFunction>().Object;
            var sut = new QueryExpression("field", function);

            // Act
            var actual = sut.With(string.Empty, null);

            // Assert
            actual.Function.Should().Be(sut.Function);
            actual.FieldName.Should().Be(sut.FieldName);
        }

        [Fact]
        public void ToBuilder_Returns_CustomBuilder_When_Available()
        {
            // Arrange
            var sut = new QueryExpressionMock();

            // Act
            var actual = sut.ToBuilder();

            // Assert
            actual.Should().BeOfType<QueryExpressionBuilderMock>();
        }

        [Fact]
        public void ToBuilder_Returns_DefaultBuilder_When_CustomBuilder_Is_Not_Available()
        {
            // Arrange
            var sut = new QueryExpression("fieldname");

            // Act
            var actual = sut.ToBuilder();

            // Assert
            actual.Should().BeOfType<QueryExpressionBuilder>();
        }

        [ExcludeFromCodeCoverage]
        private class QueryExpressionMock : ICustomQueryExpression
        {
            public string FieldName { get; set; } = "";
            public IQueryExpressionFunction? Function { get; set; }

            public IQueryExpressionBuilder CreateBuilder()
            {
                return new QueryExpressionBuilderMock
                {
                    FieldName = FieldName,
                    Function = Function
                };
            }
        }

        [ExcludeFromCodeCoverage]
        private class QueryExpressionBuilderMock : IQueryExpressionBuilder
        {
            public IQueryExpressionFunction? Function { get; set; }
            public string FieldName { get; set; } = "";

            public IQueryExpression Build()
            {
                return new QueryExpressionMock
                {
                    FieldName = FieldName,
                    Function = Function
                };
            }
        }
    }
}
