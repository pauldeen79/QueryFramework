namespace ExpressionFramework.Core.Tests.FunctionEvaluators;

public class CoalesceFunctionEvaluatorTests
{
    [Fact]
    public void TryParse_Return_False_When_Function_Is_Not_Of_Correct_Type()
    {
        // Arrange
        var sut = new CoalesceFunctionEvaluator();
        var functionMock = new Mock<IExpressionFunction>();
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(functionMock.Object, "test", expressionEvaluatorMock.Object, out var _);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type()
    {
        // Arrange
        var sut = new CoalesceFunctionEvaluator();
        var expressionMock1 = new Mock<IConstantExpression>();
        expressionMock1.SetupGet(x => x.Value).Returns((object?)null);
        var expressionMock2 = new Mock<IConstantExpression>();
        expressionMock2.SetupGet(x => x.Value).Returns("some non-null return value");
        var function = new CoalesceFunction(null, new[] { expressionMock1.Object, expressionMock2.Object });
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();
        expressionEvaluatorMock.Setup(x => x.Evaluate(It.IsAny<object?>(), It.IsAny<IConstantExpression>()))
                               .Returns<object?, IExpression>((_, expression) => ((IConstantExpression)expression).Value);

        // Act
        var actual = sut.TryEvaluate(function, "test", expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be("some non-null return value");
    }
}
