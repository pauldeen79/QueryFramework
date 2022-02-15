namespace ExpressionFramework.Core.Tests.FunctionEvaluators;

public class UpperFunctionEvaluatorTests
{
    [Fact]
    public void TryParse_Return_False_When_Function_Is_Not_Correct()
    {
        // Arrange
        var sut = new UpperFunctionEvaluator();
        var functionMock = new Mock<IExpressionFunction>();
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(functionMock.Object, "test", expressionEvaluatorCallbackMock.Object, out var _);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Correct()
    {
        // Arrange
        var sut = new UpperFunctionEvaluator();
        var function = new UpperFunction(null);
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(function, "test", expressionEvaluatorCallbackMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be("TEST");
    }

    [Fact]
    public void TryParse_Gives_Empty_String_When_Value_Is_Null()
    {
        // Arrange
        var sut = new UpperFunctionEvaluator();
        var function = new UpperFunction(null);
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(function, null, expressionEvaluatorCallbackMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(string.Empty);
    }
}
