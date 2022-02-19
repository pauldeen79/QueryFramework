namespace QueryFramework.Core.Tests.Extensions;

public class QueryConditionExtensionsTests
{
    [Fact]
    public void Can_Create_QueryCondition_Using_DoesContain()
        => AssertQueryCondition(x => x.DoesContain("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.Contains);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesEndWith()
        => AssertQueryCondition(x => x.DoesEndWith("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.EndsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsEqualTo()
        => AssertQueryCondition(x => x.IsEqualTo("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.Equal);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterOrEqualThan()
        => AssertQueryCondition(x => x.IsGreaterOrEqualThan("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.GreaterOrEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsGreaterThan()
        => AssertQueryCondition(x => x.IsGreaterThan("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.Greater);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNullOrEmpty()
        => AssertQueryCondition(x => x.IsNotNullOrEmpty()/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.IsNotNullOrEmpty);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotNull()
        => AssertQueryCondition(x => x.IsNotNull()/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.IsNotNull);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNullOrEmpty()
        => AssertQueryCondition(x => x.IsNullOrEmpty()/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.IsNullOrEmpty);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNull()
        => AssertQueryCondition(x => x.IsNull()/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.IsNull);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerOrEqualThan()
        => AssertQueryCondition(x => x.IsSmallerOrEqualThan("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.SmallerOrEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsSmallerThan()
        => AssertQueryCondition(x => x.IsSmallerThan("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.Smaller);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotContain()
        => AssertQueryCondition(x => x.DoesNotContain("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.NotContains);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotEndWith()
        => AssertQueryCondition(x => x.DoesNotEndWith("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.NotEndsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_IsNotEqualTo()
        => AssertQueryCondition(x => x.IsNotEqualTo("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.NotEqual);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesNotStartWith()
        => AssertQueryCondition(x => x.DoesNotStartWith("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.NotStartsWith);

    [Fact]
    public void Can_Create_QueryCondition_Using_DoesStartWith()
        => AssertQueryCondition(x => x.DoesStartWith("value")/*.WithOpenBracket().WithCloseBracket().WithCombination(QueryCombination.Or)*/, Operator.StartsWith);

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
