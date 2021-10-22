using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core.Observable;
using Xunit;

namespace QueryFramework.Core.Tests.Observable
{
    [ExcludeFromCodeCoverage]
    public class ObservableQueryConditionBuilderTests
    {
        [Fact]
        public void Can_Create_QueryCondition_From_Builder()
        {
            // Arrange
            var sut = new ObservableQueryConditionBuilder
            {
                CloseBracket = true,
                Combination = QueryCombination.Or,
                Field = new ObservableQueryExpressionBuilder { Expression = "expression", FieldName = "fieldname" },
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
            var actual = new ObservableQueryConditionBuilder(input);

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
            var actual = new ObservableQueryConditionBuilder(closeBracket: true,
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
            var actual = new ObservableQueryConditionBuilder(closeBracket: true,
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

        [Fact]
        public void Can_Observe_Change_On_All_Properties()
        {
            // Arrange
            var sut = new ObservableQueryConditionBuilder();
            var changedPropertiesList = new List<string>();
            sut.PropertyChanged += (sender, args) =>
            {
                changedPropertiesList.Add(args.PropertyName);
            };

            // Act
            sut.CloseBracket = true;
            sut.Combination = QueryCombination.Or;
            sut.Field = new ObservableQueryExpressionBuilder("fieldname");
            sut.OpenBracket = true;
            sut.Operator = QueryOperator.EndsWith;
            sut.Value = "value";

            // Assert
            changedPropertiesList.Should().HaveCount(6);
            changedPropertiesList.ElementAt(0).Should().Be("CloseBracket");
            changedPropertiesList.ElementAt(1).Should().Be("Combination");
            changedPropertiesList.ElementAt(2).Should().Be("Field");
            changedPropertiesList.ElementAt(3).Should().Be("OpenBracket");
            changedPropertiesList.ElementAt(4).Should().Be("Operator");
            changedPropertiesList.ElementAt(5).Should().Be("Value");
        }
    }
}
