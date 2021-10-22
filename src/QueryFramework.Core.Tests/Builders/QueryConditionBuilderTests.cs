using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core.Builders;
using Xunit;

namespace QueryFramework.Core.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class QueryConditionBuilderTests
    {
        [Fact]
        public void Can_Create_QueryCondition_From_Builder()
        {
            // Arrange
            var sut = new QueryConditionBuilder
            {
                CloseBracket = true,
                Combination = QueryCombination.Or,
                Field = new QueryExpressionBuilder { Expression = "expression", FieldName = "fieldname" },
                OpenBracket = true,
                Operator = QueryOperator.Contains,
                Value = "value"
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.CloseBracket.Should().Be(sut.CloseBracket);
            actual.Combination.Should().Be(sut.Combination);
            actual.Field.Expression.Should().Be(sut.Field.Expression);
            actual.Field.FieldName.Should().Be(sut.Field.FieldName);
            actual.OpenBracket.Should().Be(sut.OpenBracket);
            actual.Operator.Should().Be(sut.Operator);
            actual.Value.Should().Be(sut.Value);
        }

        [Fact]
        public void Can_Create_QueryConditionBuilder_From_QueryCondition()
        {
            // Arrange
            var input = new QueryCondition
            (
                closeBracket: true,
                combination: QueryCombination.Or,
                field: new QueryExpression(expression: "expression", fieldName: "fieldname"),
                openBracket: true,
                queryOperator: QueryOperator.Contains,
                value: "value"
            );

            // Act
            var actual = new QueryConditionBuilder(input);

            // Assert
            actual.CloseBracket.Should().Be(input.CloseBracket);
            actual.Combination.Should().Be(input.Combination);
            actual.Field.Expression.Should().Be(input.Field.Expression);
            actual.Field.FieldName.Should().Be(input.Field.FieldName);
            actual.OpenBracket.Should().Be(input.OpenBracket);
            actual.Operator.Should().Be(input.Operator);
            actual.Value.Should().Be(input.Value);
        }

        [Fact]
        public void Can_Create_QueryConditionBuilder_From_Values_With_Expression()
        {
            // Act
            var actual = new QueryConditionBuilder(closeBracket: true,
                                                   combination: QueryCombination.Or,
                                                   expression: new QueryExpression(expression: "expression", fieldName: "fieldname"),
                                                   openBracket: true,
                                                   queryOperator: QueryOperator.Contains,
                                                   value: "value");

            // Assert
            actual.CloseBracket.Should().Be(true);
            actual.Combination.Should().Be(QueryCombination.Or);
            actual.Field.Expression.Should().Be("expression");
            actual.Field.FieldName.Should().Be("fieldname");
            actual.OpenBracket.Should().Be(true);
            actual.Operator.Should().Be(QueryOperator.Contains);
            actual.Value.Should().Be("value");
        }

        [Fact]
        public void Can_Create_QueryConditionBuilder_From_Values_Without_Expression()
        {
            // Act
            var actual = new QueryConditionBuilder(closeBracket: true,
                                                   combination: QueryCombination.Or,
                                                   fieldName: "fieldname",
                                                   openBracket: true,
                                                   queryOperator: QueryOperator.Contains,
                                                   value: "value");

            // Assert
            actual.CloseBracket.Should().Be(true);
            actual.Combination.Should().Be(QueryCombination.Or);
            actual.Field.Expression.Should().BeNull();
            actual.Field.FieldName.Should().Be("fieldname");
            actual.OpenBracket.Should().Be(true);
            actual.Operator.Should().Be(QueryOperator.Contains);
            actual.Value.Should().Be("value");
        }
    }
}
