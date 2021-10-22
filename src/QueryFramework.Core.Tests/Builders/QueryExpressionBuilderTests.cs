using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
            var sut = new QueryExpressionBuilder
            {
                Expression = "expression",
                FieldName = "fieldname"
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.Expression.Should().Be(sut.Expression);
            actual.FieldName.Should().Be(sut.FieldName);
        }

        [Fact]
        public void Can_Create_QueryExpressionBuilder_From_QueryExpression()
        {
            // Arrange
            var input = new QueryExpression
            (
                expression: "expression",
                fieldName: "fieldname"
            );

            // Act
            var actual = new QueryExpressionBuilder(input);

            // Assert
            actual.Expression.Should().Be(input.Expression);
            actual.FieldName.Should().Be(input.FieldName);
        }

        [Fact]
        public void Can_Create_QueryExpressionBuilder_From_Values()
        {
            // Act
            var actual = new QueryExpressionBuilder(expression: "expression", fieldName: "fieldname");

            // Assert
            actual.Expression.Should().Be("expression");
            actual.FieldName.Should().Be("fieldname");
        }
    }
}
