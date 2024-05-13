namespace QueryFramework.Core.Tests;

public class ComposableEvaluatableExpressionBuilderWrapperTests
{
    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain_Constant_Value()
        => QueryConditionTest(x => x.Contains("value"), typeof(StringContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain_Delegate()
        => QueryConditionTest(x => x.Contains(() => "value"), typeof(StringContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain_Expression()
        => QueryConditionTest(x => x.Contains(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(StringContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith_Constant_Value()
        => QueryConditionTest(x => x.EndsWith("value"), typeof(EndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith_Delegate()
        => QueryConditionTest(x => x.EndsWith(() => "value"), typeof(EndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith_Expression()
        => QueryConditionTest(x => x.EndsWith(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(EndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo_Constant_Value()
        => QueryConditionTest(x => x.IsEqualTo("value"), typeof(EqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo_Delegate()
        => QueryConditionTest(x => x.IsEqualTo(() => "value"), typeof(EqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo_Expression()
        => QueryConditionTest(x => x.IsEqualTo(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(EqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan_Constant_Value()
        => QueryConditionTest(x => x.IsGreaterOrEqualThan("value"), typeof(IsGreaterOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan_Delegate()
        => QueryConditionTest(x => x.IsGreaterOrEqualThan(() => "value"), typeof(IsGreaterOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan_Expression()
        => QueryConditionTest(x => x.IsGreaterOrEqualThan(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(IsGreaterOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan_Constant_Value()
        => QueryConditionTest(x => x.IsGreaterThan("value"), typeof(IsGreaterOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan_Delegate()
        => QueryConditionTest(x => x.IsGreaterThan(() => "value"), typeof(IsGreaterOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan_Expression()
        => QueryConditionTest(x => x.IsGreaterThan(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(IsGreaterOperator));

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
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan_Constant_Value()
        => QueryConditionTest(x => x.IsSmallerOrEqualThan("value"), typeof(IsSmallerOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan_Delegate()
        => QueryConditionTest(x => x.IsSmallerOrEqualThan(() => "value"), typeof(IsSmallerOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan_Expression()
        => QueryConditionTest(x => x.IsSmallerOrEqualThan(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(IsSmallerOrEqualOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan_Constant_Value()
        => QueryConditionTest(x => x.IsSmallerThan("value"), typeof(IsSmallerOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan_Delegate()
        => QueryConditionTest(x => x.IsSmallerThan(() => "value"), typeof(IsSmallerOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan_Expression()
        => QueryConditionTest(x => x.IsSmallerThan(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(IsSmallerOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain_Constant_Value()
        => QueryConditionTest(x => x.DoesNotContain("value"), typeof(StringNotContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain_Delegate()
        => QueryConditionTest(x => x.DoesNotContain(() => "value"), typeof(StringNotContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain_Expression()
        => QueryConditionTest(x => x.DoesNotContain(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(StringNotContainsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith_Constant_Value()
        => QueryConditionTest(x => x.DoesNotEndWith("value"), typeof(NotEndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith_Delegate()
        => QueryConditionTest(x => x.DoesNotEndWith(() => "value"), typeof(NotEndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith_Expression()
        => QueryConditionTest(x => x.DoesNotEndWith(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(NotEndsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo_Constant_Value()
        => QueryConditionTest(x => x.IsNotEqualTo("value"), typeof(NotEqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo_Delegate()
        => QueryConditionTest(x => x.IsNotEqualTo(() => "value"), typeof(NotEqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo_Expression()
        => QueryConditionTest(x => x.IsNotEqualTo(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(NotEqualsOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith_Constant_Value()
        => QueryConditionTest(x => x.DoesNotStartWith("value"), typeof(NotStartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith_Delegate()
        => QueryConditionTest(x => x.DoesNotStartWith(() => "value"), typeof(NotStartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith_Expression()
        => QueryConditionTest(x => x.DoesNotStartWith(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(NotStartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith_Constant_Value()
        => QueryConditionTest(x => x.StartsWith("value"), typeof(StartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith_Delegate()
        => QueryConditionTest(x => x.StartsWith(() => "value"), typeof(StartsWithOperator));

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith_Expression()
        => QueryConditionTest(x => x.StartsWith(new TypedConstantExpressionBuilder<string>().WithValue("value")), typeof(StartsWithOperator));

    private static void QueryConditionTest(Func<ComposableEvaluatableExpressionBuilderWrapper<SingleEntityQueryBuilder>, SingleEntityQueryBuilder> builderDelegate, Type expectedOperatorType)
    {
        // Arrange
        var queryBuilder = new SingleEntityQueryBuilder();
        var wrapper = queryBuilder.Where(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName("fieldname"));

        // Act
        var actual = builderDelegate(wrapper).Filter.Conditions.Single();

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
        }
        @operator.Should().NotBeNull();
        @operator!.Build().Should().BeOfType(expectedOperatorType);
    }
}
