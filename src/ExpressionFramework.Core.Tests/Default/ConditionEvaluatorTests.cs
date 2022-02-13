namespace ExpressionFramework.Core.Tests.Default;

public class ConditionEvaluatorTests
{
    private readonly ExpressionEvaluatorMock _evaluator = new ExpressionEvaluatorMock();
    private ConditionEvaluator CreateSut() => new ConditionEvaluator(new[] { _evaluator });

    [Fact]
    public void IsItemValid_Throws_On_Unsupported_Operator()
    {
        // Arrange
        var conditionMock = new Mock<ICondition>();
        conditionMock.SetupGet(x => x.Operator)
                     .Returns((Operator)int.MaxValue);
        _evaluator.Delegate = new Func<object?, IExpression, Tuple<bool, object?>>((_, _) => new Tuple<bool, object?>(true, null));

        // Act
        CreateSut().Invoking(x => x.IsItemValid(default, conditionMock.Object))
                   .Should().ThrowExactly<ArgumentOutOfRangeException>()
                   .WithParameterName("condition")
                   .And.Message.Should().StartWith($"Unsupported operator: {int.MaxValue}");
    }

    [Fact]
    public void IsItemValid_Throws_On_Unsupported_Left_Expression()
    {
        // Arrange
        var leftExpression = new Mock<IExpression>().Object;
        var rightExpression = new Mock<IExpression>().Object;
        var conditionMock = new Mock<ICondition>();
        conditionMock.SetupGet(x => x.LeftExpression).Returns(leftExpression);
        conditionMock.SetupGet(x => x.RightExpression).Returns(rightExpression);

        // Act
        CreateSut().Invoking(x => x.IsItemValid(default, conditionMock.Object))
                   .Should().ThrowExactly<ArgumentOutOfRangeException>()
                   .WithParameterName("condition")
                   .And.Message.Should().StartWith("Unsupported left expression in condition: [IExpressionProxy]");
    }

    [Fact]
    public void IsItemValid_Throws_On_Unsupported_Right_Expression()
    {
        // Arrange
        var leftExpression = new Mock<IExpression>().Object;
        var rightExpression = new Mock<IExpression>().Object;
        var conditionMock = new Mock<ICondition>();
        conditionMock.SetupGet(x => x.LeftExpression).Returns(leftExpression);
        conditionMock.SetupGet(x => x.RightExpression).Returns(rightExpression);
        _evaluator.Delegate = new Func<object?, IExpression, Tuple<bool, object?>>((item, expression) => new Tuple<bool, object?>(expression != rightExpression, null));

        // Act
        CreateSut().Invoking(x => x.IsItemValid(default, conditionMock.Object))
                   .Should().ThrowExactly<ArgumentOutOfRangeException>()
                   .WithParameterName("condition")
                   .And.Message.Should().StartWith("Unsupported right expression in condition: [IExpressionProxy]");
    }

    [Theory]
    [InlineData("A", "A", Operator.Equal, true)]
    [InlineData("A", "a", Operator.Equal, true)]
    [InlineData("A", "b", Operator.Equal, false)]
    [InlineData("", "", Operator.Equal, true)]
    [InlineData(null, null, Operator.Equal, true)]
    [InlineData("A", "A", Operator.NotEqual, false)]
    [InlineData("A", "a", Operator.NotEqual, false)]
    [InlineData("A", "b", Operator.NotEqual, true)]
    [InlineData(1, 2, Operator.Greater, false)]
    [InlineData(2, 2, Operator.Greater, false)]
    [InlineData(3, 2, Operator.Greater, true)]
    [InlineData(1, 2, Operator.GreaterOrEqual, false)]
    [InlineData(2, 2, Operator.GreaterOrEqual, true)]
    [InlineData(3, 2, Operator.GreaterOrEqual, true)]
    [InlineData(2, 1, Operator.Smaller, false)]
    [InlineData(2, 2, Operator.Smaller, false)]
    [InlineData(2, 3, Operator.Smaller, true)]
    [InlineData(2, 1, Operator.SmallerOrEqual, false)]
    [InlineData(2, 2, Operator.SmallerOrEqual, true)]
    [InlineData(2, 3, Operator.SmallerOrEqual, true)]
    [InlineData("A", null, Operator.IsNull, false)]
    [InlineData("", null, Operator.IsNull, false)]
    [InlineData(null, null, Operator.IsNull, true)]
    [InlineData("A", null, Operator.IsNotNull, true)]
    [InlineData("", null, Operator.IsNotNull, true)]
    [InlineData(null, null, Operator.IsNotNull, false)]
    [InlineData("A", null, Operator.IsNullOrEmpty, false)]
    [InlineData("", null, Operator.IsNullOrEmpty, true)]
    [InlineData(" ", null, Operator.IsNullOrEmpty, false)]
    [InlineData(null, null, Operator.IsNullOrEmpty, true)]
    [InlineData("A", null, Operator.IsNotNullOrEmpty, true)]
    [InlineData("", null, Operator.IsNotNullOrEmpty, false)]
    [InlineData(" ", null, Operator.IsNotNullOrEmpty, true)]
    [InlineData(null, null, Operator.IsNotNullOrEmpty, false)]
    [InlineData("A", null, Operator.IsNullOrWhiteSpace, false)]
    [InlineData("", null, Operator.IsNullOrWhiteSpace, true)]
    [InlineData(" ", null, Operator.IsNullOrWhiteSpace, true)]
    [InlineData(null, null, Operator.IsNullOrWhiteSpace, true)]
    [InlineData("A", null, Operator.IsNotNullOrWhiteSpace, true)]
    [InlineData("", null, Operator.IsNotNullOrWhiteSpace, false)]
    [InlineData(" ", null, Operator.IsNotNullOrWhiteSpace, false)]
    [InlineData(null, null, Operator.IsNotNullOrWhiteSpace, false)]
    [InlineData("Pizza", "x", Operator.Contains, false)]
    [InlineData("Pizza", "a", Operator.Contains, true)]
    [InlineData("Pizza", "A", Operator.Contains, true)]
    [InlineData("Pizza", "x", Operator.NotContains, true)]
    [InlineData("Pizza", "a", Operator.NotContains, false)]
    [InlineData("Pizza", "A", Operator.NotContains, false)]
    [InlineData("Pizza", "x", Operator.StartsWith, false)]
    [InlineData("Pizza", "p", Operator.StartsWith, true)]
    [InlineData("Pizza", "P", Operator.StartsWith, true)]
    [InlineData("Pizza", "x", Operator.NotStartsWith, true)]
    [InlineData("Pizza", "p", Operator.NotStartsWith, false)]
    [InlineData("Pizza", "P", Operator.NotStartsWith, false)]
    [InlineData("Pizza", "x", Operator.EndsWith, false)]
    [InlineData("Pizza", "a", Operator.EndsWith, true)]
    [InlineData("Pizza", "A", Operator.EndsWith, true)]
    [InlineData("Pizza", "x", Operator.NotEndsWith, true)]
    [InlineData("Pizza", "a", Operator.NotEndsWith, false)]
    [InlineData("Pizza", "A", Operator.NotEndsWith, false)]
    public void IsItemValid_Returns_Correct_Result_On_Contains_Condition(object? leftValue,
                                                                         object? rightValue,
                                                                         Operator @operator,
                                                                         bool expectedResult)
    {
        // Arrange
        var leftExpressionMock = new Mock<IConstantExpression>();
        leftExpressionMock.SetupGet(x => x.Value).Returns(leftValue);
        var leftExpression = leftExpressionMock.Object;
        var rightExpressionMock = new Mock<IConstantExpression>();
        rightExpressionMock.SetupGet(x => x.Value).Returns(rightValue);
        var rightExpression = rightExpressionMock.Object;
        var conditionMock = new Mock<ICondition>();
        conditionMock.SetupGet(x => x.Operator).Returns(@operator);
        conditionMock.SetupGet(x => x.LeftExpression).Returns(leftExpression);
        conditionMock.SetupGet(x => x.RightExpression).Returns(rightExpression);
        _evaluator.Delegate = new Func<object?, IExpression, Tuple<bool, object?>>((item, expression) =>
        {
            if (expression == leftExpression)
            {
                return new Tuple<bool, object?>(true, leftExpression.Value);
            }
            if (expression == rightExpression)
            {
                return new Tuple<bool, object?>(true, rightExpression.Value);
            }

            return new Tuple<bool, object?>(false, default);
        });

        // Act
        var actual = CreateSut().IsItemValid(default, conditionMock.Object);

        // Assert
        actual.Should().Be(expectedResult);
    }
}
