namespace QueryFramework.Core.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain()
        => QueryConditionTest(x => x.DoesContain("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.Contains);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith()
        => QueryConditionTest(x => x.DoesEndWith("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.EndsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo()
        => QueryConditionTest(x => x.IsEqualTo("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.Equal);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan()
        => QueryConditionTest(x => x.IsGreaterOrEqualThan("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.GreaterOrEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan()
        => QueryConditionTest(x => x.IsGreaterThan("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.Greater);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrEmpty()
        => QueryConditionTest(x => x.IsNotNullOrEmpty()/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.IsNotNullOrEmpty);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNull()
        => QueryConditionTest(x => x.IsNotNull()/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.IsNotNull);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrEmpty()
        => QueryConditionTest(x => x.IsNullOrEmpty()/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.IsNullOrEmpty);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNull()
        => QueryConditionTest(x => x.IsNull()/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.IsNull);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan()
        => QueryConditionTest(x => x.IsSmallerOrEqualThan("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.SmallerOrEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan()
        => QueryConditionTest(x => x.IsSmallerThan("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.Smaller);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain()
        => QueryConditionTest(x => x.DoesNotContain("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.NotContains);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith()
        => QueryConditionTest(x => x.DoesNotEndWith("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.NotEndsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo()
        => QueryConditionTest(x => x.IsNotEqualTo("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.NotEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith()
        => QueryConditionTest(x => x.DoesNotStartWith("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.NotStartsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith()
        => QueryConditionTest(x => x.DoesStartWith("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.StartsWith);

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
            || expectedOperator == Operator.IsNotNull
            || expectedOperator == Operator.IsNotNullOrEmpty)
        {
            value.Should().BeNull();
        }
        else
        {
            value.Should().Be("value");
        }
        //actual.OpenBracket.Should().BeTrue();
        //actual.CloseBracket.Should().BeTrue();
        //actual.Combination.Should().Be(QueryCombination.Or);
        actual.Operator.Should().Be(expectedOperator);
    }
}
