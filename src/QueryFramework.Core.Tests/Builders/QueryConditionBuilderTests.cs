using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Extensions;
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
            var function = new Mock<IQueryExpressionFunction>().Object;
            var sut = new QueryConditionBuilder
            {
                CloseBracket = true,
                Combination = QueryCombination.Or,
                Field = new QueryExpressionBuilder { Function = function, FieldName = "fieldname" },
                OpenBracket = true,
                Operator = QueryOperator.Contains,
                Value = "value"
            };

            // Act
            var actual = sut.Build();

            // Assert
            actual.CloseBracket.Should().Be(sut.CloseBracket);
            actual.Combination.Should().Be(sut.Combination);
            actual.Field.Function.Should().Be(sut.Field.Function);
            actual.Field.FieldName.Should().Be(sut.Field.FieldName);
            actual.OpenBracket.Should().Be(sut.OpenBracket);
            actual.Operator.Should().Be(sut.Operator);
            actual.Value.Should().Be(sut.Value);
        }

        [Fact]
        public void Can_Create_QueryConditionBuilder_From_QueryCondition()
        {
            // Arrange
            var function = new Mock<IQueryExpressionFunction>().Object;
            var input = new QueryCondition
            (
                closeBracket: true,
                combination: QueryCombination.Or,
                field: new QueryExpression(function: function, fieldName: "fieldname"),
                openBracket: true,
                queryOperator: QueryOperator.Contains,
                value: "value"
            );

            // Act
            var actual = new QueryConditionBuilder(input);

            // Assert
            actual.CloseBracket.Should().Be(input.CloseBracket);
            actual.Combination.Should().Be(input.Combination);
            actual.Field.Function.Should().Be(input.Field.Function);
            actual.Field.FieldName.Should().Be(input.Field.FieldName);
            actual.OpenBracket.Should().Be(input.OpenBracket);
            actual.Operator.Should().Be(input.Operator);
            actual.Value.Should().Be(input.Value);
        }

        [Fact]
        public void Can_Create_QueryConditionBuilder_From_Values_With_Expression()
        {
            // Act
            var function = new Mock<IQueryExpressionFunction>().Object;
            var actual = new QueryConditionBuilder().WithCloseBracket()
                                                    .WithCombination(QueryCombination.Or)
                                                    .WithField(new QueryExpressionBuilder().WithFunction(function).WithFieldName("fieldname"))
                                                    .WithOpenBracket()
                                                    .WithOperator(QueryOperator.Contains)
                                                    .WithValue("value");

            // Assert
            actual.CloseBracket.Should().Be(true);
            actual.Combination.Should().Be(QueryCombination.Or);
            actual.Field.Function.Should().BeSameAs(function);
            actual.Field.FieldName.Should().Be("fieldname");
            actual.OpenBracket.Should().Be(true);
            actual.Operator.Should().Be(QueryOperator.Contains);
            actual.Value.Should().Be("value");
        }

        [Fact]
        public void Can_Create_QueryConditionBuilder_From_Values_Without_Expression()
        {
            // Act
            var actual = new QueryConditionBuilder().WithCloseBracket()
                                                    .WithCombination(QueryCombination.Or)
                                                    .WithField("fieldname")
                                                    .WithOpenBracket()
                                                    .WithOperator(QueryOperator.Contains)
                                                    .WithValue("value");

            // Assert
            actual.CloseBracket.Should().Be(true);
            actual.Combination.Should().Be(QueryCombination.Or);
            actual.Field.Function.Should().BeNull();
            actual.Field.FieldName.Should().Be("fieldname");
            actual.OpenBracket.Should().Be(true);
            actual.Operator.Should().Be(QueryOperator.Contains);
            actual.Value.Should().Be("value");
        }
    }
}
