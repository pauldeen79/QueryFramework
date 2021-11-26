using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Core.Extensions;
using Xunit;

namespace QueryFramework.Core.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTests
    {
        [Fact]
        public void Can_Create_QueryCondition_Using_DoesContain()
            => QueryConditionTest(x => x.DoesContain(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.Contains);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesEndWith()
            => QueryConditionTest(x => x.DoesEndWith(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.EndsWith);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsEqualTo()
            => QueryConditionTest(x => x.IsEqualTo(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.Equal);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan()
            => QueryConditionTest(x => x.IsGreaterOrEqualThan(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.GreaterOrEqual);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsGreaterThan()
            => QueryConditionTest(x => x.IsGreaterThan(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.Greater);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNotNullOrEmpty()
            => QueryConditionTest(x => x.IsNotNullOrEmpty(openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.IsNotNullOrEmpty);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNotNull()
            => QueryConditionTest(x => x.IsNotNull(openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.IsNotNull);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNullOrEmpty()
            => QueryConditionTest(x => x.IsNullOrEmpty(openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.IsNullOrEmpty);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNull()
            => QueryConditionTest(x => x.IsNull(openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.IsNull);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsLowerOrEqualThan()
            => QueryConditionTest(x => x.IsLowerOrEqualThan(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.LowerOrEqual);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsLowerThan()
            => QueryConditionTest(x => x.IsLowerThan(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.Lower);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesNotContain()
            => QueryConditionTest(x => x.DoesNotContain(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.NotContains);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesNotEndWith()
            => QueryConditionTest(x => x.DoesNotEndWith(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.NotEndsWith);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNotEqualTo()
            => QueryConditionTest(x => x.IsNotEqualTo(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.NotEqual);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesNotStartWith()
            => QueryConditionTest(x => x.DoesNotStartWith(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.NotStartsWith);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesStartWith()
            => QueryConditionTest(x => x.DoesStartWith(value: "value", openBracket: true, closeBracket: true, combination: QueryCombination.Or), QueryOperator.StartsWith);

        private static void QueryConditionTest(Func<string, IQueryCondition> func, QueryOperator expectedOperator)
        {
            // Arrange
            var queryExpressionFieldName = "fieldname";

            // Act
            var actual = func(queryExpressionFieldName);

            // Assert
            actual.Field.FieldName.Should().Be("fieldname");
            if (expectedOperator == QueryOperator.IsNull
                || expectedOperator == QueryOperator.IsNullOrEmpty
                || expectedOperator == QueryOperator.IsNotNull
                || expectedOperator == QueryOperator.IsNotNullOrEmpty)
            {
                actual.Value.Should().BeNull();
            }
            else
            {
                actual.Value.Should().Be("value");
            }
            actual.OpenBracket.Should().BeTrue();
            actual.CloseBracket.Should().BeTrue();
            actual.Combination.Should().Be(QueryCombination.Or);
            actual.Operator.Should().Be(expectedOperator);
        }
    }
}
