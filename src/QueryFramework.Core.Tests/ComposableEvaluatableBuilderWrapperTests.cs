﻿namespace QueryFramework.Core.Tests;

public class ComposableEvaluatableBuilderWrapperTests
{
    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain()
        => QueryConditionTest(x => x.Contains("value"), typeof(StringContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith()
        => QueryConditionTest(x => x.EndsWith("value"), typeof(EndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo()
        => QueryConditionTest(x => x.IsEqualTo("value"), typeof(EqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan()
        => QueryConditionTest(x => x.IsGreaterOrEqualThan("value"), typeof(IsGreaterOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan()
        => QueryConditionTest(x => x.IsGreaterThan("value"), typeof(IsGreaterOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrEmpty()
        => QueryConditionTest(x => x.IsNotNullOrEmpty(), typeof(IsNotNullOrEmptyOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrWhiteSpace()
    => QueryConditionTest(x => x.IsNotNullOrWhiteSpace(), typeof(IsNotNullOrWhiteSpaceOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNull()
        => QueryConditionTest(x => x.IsNotNull(), typeof(IsNotNullOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrEmpty()
        => QueryConditionTest(x => x.IsNullOrEmpty(), typeof(IsNullOrEmptyOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrWhiteSpace()
        => QueryConditionTest(x => x.IsNullOrWhiteSpace(), typeof(IsNullOrWhiteSpaceOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNull()
        => QueryConditionTest(x => x.IsNull(), typeof(IsNullOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan()
        => QueryConditionTest(x => x.IsSmallerOrEqualThan("value"), typeof(IsSmallerOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan()
        => QueryConditionTest(x => x.IsSmallerThan("value"), typeof(IsSmallerOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain()
        => QueryConditionTest(x => x.DoesNotContain("value"), typeof(StringNotContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith()
        => QueryConditionTest(x => x.DoesNotEndWith("value"), typeof(NotEndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo()
        => QueryConditionTest(x => x.IsNotEqualTo("value"), typeof(NotEqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith()
        => QueryConditionTest(x => x.DoesNotStartWith("value"), typeof(NotStartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith()
        => QueryConditionTest(x => x.StartsWith("value"), typeof(StartsWithOperator));

    private static void QueryConditionTest(Func<ComposableEvaluatableBuilderWrapper<SingleEntityQueryBuilder>, SingleEntityQueryBuilder> func, Type expectedOperatorType)
    {
        // Arrange
        var queryBuilder = new SingleEntityQueryBuilder();
        var wrapper = queryBuilder.Where("fieldname");

        // Act
        var actual = func(wrapper).Filter.Conditions.Single();

        // Assert
        var leftExpression = actual.LeftExpression;
        var rightExpression = actual.RightExpression;
        var @operator = actual.Operator;
        var field = leftExpression as FieldExpressionBuilder;
        var value = (rightExpression as ConstantExpressionBuilder)?.Value;
        ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.Should().Be("fieldname");
        if (expectedOperatorType == typeof(IsNullOperator)
            || expectedOperatorType == typeof(IsNullOrEmptyOperator)
            || expectedOperatorType == typeof(IsNullOrWhiteSpaceOperator)
            || expectedOperatorType == typeof(IsNotNullOperator)
            || expectedOperatorType == typeof(IsNotNullOrEmptyOperator)
            || expectedOperatorType == typeof(IsNotNullOrWhiteSpaceOperator))
        {
            value.Should().BeNull();
        }
        else
        {
            value.Should().Be("value");
        }
        @operator.Should().NotBeNull();
        @operator!.Build().Should().BeOfType(expectedOperatorType);
    }
}
