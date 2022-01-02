using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Core.Builders;
using QueryFramework.Core.Extensions;
using Xunit;

namespace QueryFramework.Core.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class QueryConditionExtensionsTests
    {
        [Fact]
        public void Can_Use_With_ExtensionMethod_To_Modify_QueryCondition()
        {
            // Arrange
            var sut = new QueryCondition("field", QueryOperator.Contains, "value", false, false, QueryCombination.And);

            // Act
            var actual = sut.With(true, true, QueryCombination.Or);

            // Assert
            actual.Field.Should().Be(sut.Field);
            actual.Operator.Should().Be(sut.Operator);
            actual.Value.Should().Be(sut.Value);
            actual.OpenBracket.Should().BeTrue();
            actual.CloseBracket.Should().BeTrue();
            actual.Combination.Should().Be(QueryCombination.Or);
        }

        [Fact]
        public void Can_Use_With_ExtensionMethod_With_DefaultValues_To_Create_Instance_With_Same_Values()
        {
            // Arrange
            var sut = new QueryCondition("field", QueryOperator.Contains, "value", false, false, QueryCombination.And);

            // Act
            var actual = sut.With(null, null, null);

            // Assert
            actual.Field.Should().Be(sut.Field);
            actual.Operator.Should().Be(sut.Operator);
            actual.Value.Should().Be(sut.Value);
            actual.OpenBracket.Should().Be(sut.OpenBracket);
            actual.CloseBracket.Should().Be(sut.CloseBracket);
            actual.Combination.Should().Be(sut.Combination);
        }

        [Fact]
        public void Can_Get_CustomBuilder_Using_GetBuilder()
        {
            // Arrange
            var sut = new QueryConditionMock();

            // Act
            var actual = sut.GetBuilder();

            // Assert
            actual.Should().BeOfType<QueryConditionBuilderMock>();
        }

        [Fact]
        public void Can_Get_DefaultBuilder_Using_GetBuilder()
        {
            // Arrange
            var sut = new QueryCondition("field", QueryOperator.Contains, "value", false, false, QueryCombination.And);

            // Act
            var actual = sut.GetBuilder();

            // Assert
            actual.Should().BeOfType<QueryConditionBuilder>();
        }

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesContain()
            => AssertQueryCondition(x => x.DoesContain("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.Contains);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesEndWith()
            => AssertQueryCondition(x => x.DoesEndWith("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.EndsWith);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsEqualTo()
            => AssertQueryCondition(x => x.IsEqualTo("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.Equal);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan()
            => AssertQueryCondition(x => x.IsGreaterOrEqualThan("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.GreaterOrEqual);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsGreaterThan()
            => AssertQueryCondition(x => x.IsGreaterThan("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.Greater);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNotNullOrEmpty()
            => AssertQueryCondition(x => x.IsNotNullOrEmpty().WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.IsNotNullOrEmpty);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNotNull()
            => AssertQueryCondition(x => x.IsNotNull().WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.IsNotNull);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNullOrEmpty()
            => AssertQueryCondition(x => x.IsNullOrEmpty().WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.IsNullOrEmpty);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNull()
            => AssertQueryCondition(x => x.IsNull().WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.IsNull);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsLowerOrEqualThan()
            => AssertQueryCondition(x => x.IsLowerOrEqualThan("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.LowerOrEqual);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsLowerThan()
            => AssertQueryCondition(x => x.IsLowerThan("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.Lower);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesNotContain()
            => AssertQueryCondition(x => x.DoesNotContain("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.NotContains);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesNotEndWith()
            => AssertQueryCondition(x => x.DoesNotEndWith("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.NotEndsWith);

        [Fact]
        public void Can_Create_QueryCondition_Using_IsNotEqualTo()
            => AssertQueryCondition(x => x.IsNotEqualTo("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.NotEqual);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesNotStartWith()
            => AssertQueryCondition(x => x.DoesNotStartWith("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.NotStartsWith);

        [Fact]
        public void Can_Create_QueryCondition_Using_DoesStartWith()
            => AssertQueryCondition(x => x.DoesStartWith("value").WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or), QueryOperator.StartsWith);

        private static void AssertQueryCondition(Func<IQueryExpressionBuilder, IQueryConditionBuilder> func, QueryOperator expectedOperator)
        {
            // Arrange
            var queryExpression = new QueryExpressionBuilder { FieldName = "fieldName" };

            // Act
            var actual = func(queryExpression);

            // Assert
            actual.Field.FieldName.Should().Be("fieldName");
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

        [ExcludeFromCodeCoverage]
        private class QueryConditionMock : ICustomQueryCondition
        {
            public bool OpenBracket { get; set; }
            public bool CloseBracket { get; set; }
            public IQueryExpression Field { get; set; } = new QueryExpression("Field");

            public QueryOperator Operator { get; set; }

            public object? Value { get; set; }

            public QueryCombination Combination { get; set; }

            public IQueryConditionBuilder CreateBuilder()
            {
                return new QueryConditionBuilderMock();
            }

            public IQueryCondition With(bool? openBracket, bool? closeBracket, QueryCombination? combination)
            {
                return new QueryConditionMock
                {
                    OpenBracket = openBracket ?? OpenBracket,
                    CloseBracket = closeBracket ?? CloseBracket,
                    Combination = combination ?? Combination
                };
            }
        }

        [ExcludeFromCodeCoverage]
        private class QueryConditionBuilderMock : IQueryConditionBuilder
        {
            public bool CloseBracket { get; set; }
            public QueryCombination Combination { get; set; }
            public IQueryExpressionBuilder Field { get; set; } = new QueryExpressionBuilder();
            public bool OpenBracket { get; set; }
            public QueryOperator Operator { get; set; }
            public object? Value { get; set; }

            public IQueryCondition Build()
            {
                return new QueryConditionMock
                {
                    CloseBracket = CloseBracket,
                    Combination = Combination,
                    Field = Field.Build(),
                    OpenBracket = OpenBracket,
                    Operator = Operator,
                    Value = Value
                };
            }
        }
    }
}
