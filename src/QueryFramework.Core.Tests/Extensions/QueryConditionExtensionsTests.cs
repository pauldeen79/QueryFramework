namespace QueryFramework.Core.Tests.Extensions;

public class QueryConditionExtensionsTests
{
    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain()
        => AssertQueryCondition(x => x.DoesContain("value"), typeof(StringContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith()
        => AssertQueryCondition(x => x.DoesEndWith("value"), typeof(EndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo()
        => AssertQueryCondition(x => x.IsEqualTo("value"), typeof(EqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan()
        => AssertQueryCondition(x => x.IsGreaterOrEqualThan("value"), typeof(IsGreaterOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan()
        => AssertQueryCondition(x => x.IsGreaterThan("value"), typeof(IsGreaterOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrEmpty()
        => AssertQueryCondition(x => x.IsNotNullOrEmpty(), typeof(IsNotNullOrEmptyOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrWhiteSpace()
        => AssertQueryCondition(x => x.IsNotNullOrWhiteSpace(), typeof(IsNotNullOrWhiteSpaceOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNull()
        => AssertQueryCondition(x => x.IsNotNull(), typeof(IsNotNullOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrEmpty()
        => AssertQueryCondition(x => x.IsNullOrEmpty(), typeof(IsNullOrEmptyOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrWhiteSpace()
        => AssertQueryCondition(x => x.IsNullOrWhiteSpace(), typeof(IsNullOrWhiteSpaceOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNull()
        => AssertQueryCondition(x => x.IsNull(), typeof(IsNullOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan()
        => AssertQueryCondition(x => x.IsSmallerOrEqualThan("value"), typeof(IsSmallerOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan()
        => AssertQueryCondition(x => x.IsSmallerThan("value"), typeof(IsSmallerOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain()
        => AssertQueryCondition(x => x.DoesNotContain("value"), typeof(StringNotContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith()
        => AssertQueryCondition(x => x.DoesNotEndWith("value"), typeof(NotEndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo()
        => AssertQueryCondition(x => x.IsNotEqualTo("value"), typeof(NotEqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith()
        => AssertQueryCondition(x => x.DoesNotStartWith("value"), typeof(NotStartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith()
        => AssertQueryCondition(x => x.DoesStartWith("value"), typeof(StartsWithOperator));

    private static void AssertQueryCondition(Func<ExpressionBuilder, EvaluatableBuilder> func, Type expectedOperatorType)
    {
        // Arrange
        var queryExpression = new FieldExpressionBuilder().WithFieldName("fieldName");

        // Act
        var actual = func(queryExpression);

        // Assert
        var leftExpression = actual.GetLeftExpression();
        var rightExpression = actual.TryGetRightExpression();
        var @operator = actual.GetOperator();
        var field = leftExpression as FieldExpressionBuilder;
        var value = (rightExpression as ConstantExpressionBuilder)?.Value;
        ((ConstantExpressionBuilder)field!.FieldNameExpression).Value.Should().Be("fieldName");
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
