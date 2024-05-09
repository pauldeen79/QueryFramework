namespace QueryFramework.Core.Tests.Extensions;

public class QueryConditionExtensionsTests
{
    [Fact]
    public void Can_Create_QueryCondition_Using_Contains_Constant_Value()
        => AssertQueryCondition(x => x.Contains("value"), typeof(StringContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_Contains_Delegate()
        => AssertQueryCondition(x => x.Contains("value"), typeof(StringContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_Contains_Expression()
        => AssertQueryCondition(x => x.Contains(new TypedDelegateExpressionBuilder<string>().WithValue(_ => "value")), typeof(StringContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_EndsWith_Constant_Value()
        => AssertQueryCondition(x => x.EndsWith("value"), typeof(EndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_EndsWith_Delegate()
        => AssertQueryCondition(x => x.EndsWith(() => "value"), typeof(EndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_EndsWith_Expression()
        => AssertQueryCondition(x => x.EndsWith(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(EndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo_Constant_Value()
        => AssertQueryCondition(x => x.IsEqualTo("value"), typeof(EqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo_Delegate()
        => AssertQueryCondition(x => x.IsEqualTo(() => "value"), typeof(EqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo_Expression()
        => AssertQueryCondition(x => x.IsEqualTo(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(EqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan_Constant_Value()
        => AssertQueryCondition(x => x.IsGreaterOrEqualThan("value"), typeof(IsGreaterOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan_Delegate()
        => AssertQueryCondition(x => x.IsGreaterOrEqualThan(() => "value"), typeof(IsGreaterOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan_Expression()
        => AssertQueryCondition(x => x.IsGreaterOrEqualThan(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(IsGreaterOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan_Constant_Value()
        => AssertQueryCondition(x => x.IsGreaterThan("value"), typeof(IsGreaterOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan_Delegate()
        => AssertQueryCondition(x => x.IsGreaterThan(() => "value"), typeof(IsGreaterOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan_Expression()
        => AssertQueryCondition(x => x.IsGreaterThan(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(IsGreaterOperator));

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
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan_Constant_Value()
        => AssertQueryCondition(x => x.IsSmallerOrEqualThan("value"), typeof(IsSmallerOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan_Delegate()
        => AssertQueryCondition(x => x.IsSmallerOrEqualThan(() => "value"), typeof(IsSmallerOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan_Expression()
        => AssertQueryCondition(x => x.IsSmallerOrEqualThan(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(IsSmallerOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan_Constant_Value()
        => AssertQueryCondition(x => x.IsSmallerThan("value"), typeof(IsSmallerOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan_Delegate()
        => AssertQueryCondition(x => x.IsSmallerThan(() => "value"), typeof(IsSmallerOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan_Expression()
        => AssertQueryCondition(x => x.IsSmallerThan(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(IsSmallerOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain_Constant_Value()
        => AssertQueryCondition(x => x.DoesNotContain("value"), typeof(StringNotContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain_Delegate()
        => AssertQueryCondition(x => x.DoesNotContain(() => "value"), typeof(StringNotContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain_Expression()
        => AssertQueryCondition(x => x.DoesNotContain(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(StringNotContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith_Constant_Value()
        => AssertQueryCondition(x => x.DoesNotEndWith("value"), typeof(NotEndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith_Delegate()
        => AssertQueryCondition(x => x.DoesNotEndWith(() => "value"), typeof(NotEndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith_Expression()
        => AssertQueryCondition(x => x.DoesNotEndWith(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(NotEndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo_Constant_Value()
        => AssertQueryCondition(x => x.IsNotEqualTo("value"), typeof(NotEqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo_Delegate()
        => AssertQueryCondition(x => x.IsNotEqualTo(() => "value"), typeof(NotEqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo_Expression()
        => AssertQueryCondition(x => x.IsNotEqualTo(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(NotEqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith_Constant_Value()
        => AssertQueryCondition(x => x.DoesNotStartWith("value"), typeof(NotStartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith_Delegate()
        => AssertQueryCondition(x => x.DoesNotStartWith(() => "value"), typeof(NotStartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith_Expression()
        => AssertQueryCondition(x => x.DoesNotStartWith(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(NotStartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_StartsWith_Constant_Value()
        => AssertQueryCondition(x => x.StartsWith("value"), typeof(StartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_StartsWith_Delegate()
        => AssertQueryCondition(x => x.StartsWith(() => "value"), typeof(StartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_StartsWith_Expression()
        => AssertQueryCondition(x => x.StartsWith(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(StartsWithOperator));

    private static void AssertQueryCondition(Func<ExpressionBuilder, ComposableEvaluatableBuilder> builderDelegate, Type expectedOperatorType)
    {
        // Arrange
        var queryExpression = new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("fieldName");

        // Act
        var actual = builderDelegate(queryExpression);

        // Assert
        var leftExpression = actual.LeftExpression;
        var rightExpression = actual.RightExpression;
        var @operator = actual.Operator;
        var field = leftExpression as FieldExpressionBuilder;
        if (rightExpression is TypedDelegateExpressionBuilder<string> typedDelegateExpressionBuilder)
        {
            typedDelegateExpressionBuilder.Value.Should().NotBeNull();

            var value = typedDelegateExpressionBuilder.Value.Invoke(null); //context is ignored
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
        }
        else
        {
            var value = (rightExpression as ConstantExpressionBuilder)?.Value
                ?? (rightExpression as TypedConstantExpressionBuilder<string>)?.Value;
            ((TypedConstantExpressionBuilder<string>)field!.FieldNameExpression).Value.Should().Be("fieldName");
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
        }
        @operator.Should().NotBeNull();
        @operator!.Build().Should().BeOfType(expectedOperatorType);
    }
}
