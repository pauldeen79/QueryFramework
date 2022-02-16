namespace ExpressionFramework.Core.Tests.FunctionEvaluators;

public class LeftFunctionEvaluatorTests
{
    [Fact]
    public void TryParse_Return_False_When_Function_Is_Not_Of_Correct_Type()
    {
        // Arrange
        var sut = new LeftFunctionEvaluator();
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
        var sut = new LeftFunctionEvaluator();
        var function = new LeftFunction(1, new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, "test", expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be("t");
    }

    [Fact]
    public void TryParse_Gives_Full_String_When_Length_Is_Too_Long()
    {
        // Arrange
        var sut = new LeftFunctionEvaluator();
        var function = new LeftFunction(5, new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, "test", expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be("test");
    }

    [Fact]
    public void TryParse_Gives_Full_String_When_Value_Is_Null()
    {
        // Arrange
        var sut = new LeftFunctionEvaluator();
        var function = new LeftFunction(2, new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, null, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(string.Empty);
    }
}
