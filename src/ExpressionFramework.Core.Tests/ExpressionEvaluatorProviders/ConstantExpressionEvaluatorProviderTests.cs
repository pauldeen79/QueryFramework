namespace ExpressionFramework.Core.Tests.ExpressionEvaluatorProviders;

public class ConstantExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryEvaluate_Returns_False_When_Expression_Is_Not_A_ConstantExpression()
    {
        // Arrange
        var sut = new ConstantExpressionEvaluatorProvider();
        var expressionMock = new Mock<IExpression>();
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorMock.Object, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryEvaluate_Returns_True_When_Expression_Is_A_ConstantExpression()
    {
        // Arrange
        var sut = new ConstantExpressionEvaluatorProvider();
        var expressionMock = new Mock<IConstantExpression>();
        expressionMock.SetupGet(x => x.Value).Returns(12345);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorMock.Object, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be(12345);
    }
}
