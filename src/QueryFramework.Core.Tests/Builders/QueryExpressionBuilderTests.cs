using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Core.Builders;
using Xunit;

namespace QueryFramework.Core.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class QueryExpressionBuilderTests
    {
        [Fact]
        public void Can_Create_QueryExpression_From_Builder()
        {
            // Arrange
            var function = new Mock<IQueryExpressionFunction>().Object;
            var sut = new QueryExpressionBuilder
            {
                Function = function,
                FieldName = "fieldname"
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.Function.Should().BeSameAs(sut.Function);
            actual.FieldName.Should().Be(sut.FieldName);
        }

        [Fact]
        public void Can_Create_QueryExpressionBuilder_From_QueryExpression()
        {
            // Arrange
            var function = new Mock<IQueryExpressionFunction>().Object;
            var input = new QueryExpression
            (
                function: function,
                fieldName: "fieldname"
            );

            // Act
            var actual = new QueryExpressionBuilder(input);

            // Assert
            actual.Function.Should().BeSameAs(input.Function);
            actual.FieldName.Should().Be(input.FieldName);
        }

        [Fact]
        public void Can_Create_QueryExpressionBuilder_From_Values()
        {
            // Act
            var function = new Mock<IQueryExpressionFunction>().Object;
            var actual = new QueryExpressionBuilder(function: function, fieldName: "fieldname");

            // Assert
            actual.Function.Should().BeSameAs(function);
            actual.FieldName.Should().Be("fieldname");
        }
    }
}
