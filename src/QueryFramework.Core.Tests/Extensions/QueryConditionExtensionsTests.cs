namespace QueryFramework.Core.Tests.Extensions;

public class QueryConditionExtensionsTests
{
    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain()
        => AssertQueryCondition(x => x.DoesContain("value"), Operator.Contains);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith()
        => AssertQueryCondition(x => x.DoesEndWith("value"), Operator.EndsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo()
        => AssertQueryCondition(x => x.IsEqualTo("value"), Operator.Equal);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan()
        => AssertQueryCondition(x => x.IsGreaterOrEqualThan("value"), Operator.GreaterOrEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan()
        => AssertQueryCondition(x => x.IsGreaterThan("value"), Operator.Greater);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrEmpty()
        => AssertQueryCondition(x => x.IsNotNullOrEmpty(), Operator.IsNotNullOrEmpty);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrWhiteSpace()
        => AssertQueryCondition(x => x.IsNotNullOrWhiteSpace(), Operator.IsNotNullOrWhiteSpace);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNull()
        => AssertQueryCondition(x => x.IsNotNull(), Operator.IsNotNull);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrEmpty()
        => AssertQueryCondition(x => x.IsNullOrEmpty(), Operator.IsNullOrEmpty);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrWhiteSpace()
        => AssertQueryCondition(x => x.IsNullOrWhiteSpace(), Operator.IsNullOrWhiteSpace);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNull()
        => AssertQueryCondition(x => x.IsNull(), Operator.IsNull);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan()
        => AssertQueryCondition(x => x.IsSmallerOrEqualThan("value"), Operator.SmallerOrEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan()
        => AssertQueryCondition(x => x.IsSmallerThan("value"), Operator.Smaller);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain()
        => AssertQueryCondition(x => x.DoesNotContain("value"), Operator.NotContains);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith()
        => AssertQueryCondition(x => x.DoesNotEndWith("value"), Operator.NotEndsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo()
        => AssertQueryCondition(x => x.IsNotEqualTo("value"), Operator.NotEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith()
        => AssertQueryCondition(x => x.DoesNotStartWith("value"), Operator.NotStartsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith()
        => AssertQueryCondition(x => x.DoesStartWith("value"), Operator.StartsWith);

    private static void AssertQueryCondition(Func<IExpressionBuilder, IConditionBuilder> func, Operator expectedOperator)
    {
        // Arrange
        var queryExpression = new FieldExpressionBuilder { FieldName = "fieldName" };

        // Act
        var actual = func(queryExpression);

        // Assert
        var field = actual.LeftExpression as FieldExpressionBuilder;
        var value = (actual.RightExpression as ConstantExpressionBuilder)?.Value;
        field?.FieldName.Should().Be("fieldName");
        if (expectedOperator == Operator.IsNull
            || expectedOperator == Operator.IsNullOrEmpty
            || expectedOperator == Operator.IsNullOrWhiteSpace
            || expectedOperator == Operator.IsNotNull
            || expectedOperator == Operator.IsNotNullOrEmpty
            || expectedOperator == Operator.IsNotNullOrWhiteSpace)
        {
            value.Should().BeNull();
        }
        else
        {
            value.Should().Be("value");
        }
        actual.Operator.Should().Be(expectedOperator);
    }
}
