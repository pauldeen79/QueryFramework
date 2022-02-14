namespace ExpressionFramework.Core.Tests.ExpressionEvaluators;

public class EmptyExpressionEvaluatorTests
{
    [Fact]
    public void TryEvaluate_Returns_False_When_Expression_Is_Not_A_EmptyExpression()
    {
        // Arrange
        var sut = new EmptyExpressionEvaluator();
        var expressionMock = new Mock<IExpression>();
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorCallbackMock.Object, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryEvaluate_Returns_True_When_Expression_Is_A_EmptyExpression()
    {
        // Arrange
        var sut = new EmptyExpressionEvaluator();
        var expressionMock = new Mock<IEmptyExpression>();
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorCallbackMock.Object, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().BeNull();
    }
}
