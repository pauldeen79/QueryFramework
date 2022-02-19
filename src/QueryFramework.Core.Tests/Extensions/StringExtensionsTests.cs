namespace QueryFramework.Core.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain()
        => QueryConditionTest(x => x.DoesContain("value"), Operator.Contains);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith()
        => QueryConditionTest(x => x.DoesEndWith("value"), Operator.EndsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo()
        => QueryConditionTest(x => x.IsEqualTo("value"), Operator.Equal);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan()
        => QueryConditionTest(x => x.IsGreaterOrEqualThan("value"), Operator.GreaterOrEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan()
        => QueryConditionTest(x => x.IsGreaterThan("value"), Operator.Greater);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrEmpty()
        => QueryConditionTest(x => x.IsNotNullOrEmpty(), Operator.IsNotNullOrEmpty);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrWhiteSpace()
    => QueryConditionTest(x => x.IsNotNullOrWhiteSpace(), Operator.IsNotNullOrWhiteSpace);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNull()
        => QueryConditionTest(x => x.IsNotNull(), Operator.IsNotNull);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrEmpty()
        => QueryConditionTest(x => x.IsNullOrEmpty(), Operator.IsNullOrEmpty);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrWhiteSpace()
        => QueryConditionTest(x => x.IsNullOrWhiteSpace(), Operator.IsNullOrWhiteSpace);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNull()
        => QueryConditionTest(x => x.IsNull(), Operator.IsNull);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan()
        => QueryConditionTest(x => x.IsSmallerOrEqualThan("value"), Operator.SmallerOrEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan()
        => QueryConditionTest(x => x.IsSmallerThan("value"), Operator.Smaller);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain()
        => QueryConditionTest(x => x.DoesNotContain("value"), Operator.NotContains);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith()
        => QueryConditionTest(x => x.DoesNotEndWith("value"), Operator.NotEndsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo()
        => QueryConditionTest(x => x.IsNotEqualTo("value"), Operator.NotEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith()
        => QueryConditionTest(x => x.DoesNotStartWith("value"), Operator.NotStartsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith()
        => QueryConditionTest(x => x.DoesStartWith("value"), Operator.StartsWith);

    private static void QueryConditionTest(Func<string, IConditionBuilder> func, Operator expectedOperator)
    {
        // Arrange
        var queryExpressionFieldName = "fieldname";

        // Act
        var actual = func(queryExpressionFieldName);

        // Assert
        var field = actual.LeftExpression as FieldExpressionBuilder;
        var value = (actual.RightExpression as ConstantExpressionBuilder)?.Value;
        field?.FieldName.Should().Be("fieldname");
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
