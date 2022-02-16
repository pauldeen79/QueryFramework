namespace ExpressionFramework.Core.Tests.ExpressionEvaluatorProviders;

public class FieldExpressionEvaluatorProviderTests
{
    [Fact]
    public void TryEvaluate_Returns_False_When_Expression_Is_Not_A_FieldExpression()
    {
        // Arrange
        var valueProviderMock = new Mock<IValueProvider>();
        var sut = new FieldExpressionEvaluatorProvider(valueProviderMock.Object);
        var expressionMock = new Mock<IExpression>();
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorMock.Object, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryEvaluate_Returns_True_When_Expression_Is_A_FieldExpression()
    {
        // Arrange
        var valueProviderMock = new Mock<IValueProvider>();
        var sut = new FieldExpressionEvaluatorProvider(valueProviderMock.Object);
        var expressionMock = new Mock<IFieldExpression>();
        expressionMock.SetupGet(x => x.Function).Returns(default(IExpressionFunction));
        expressionMock.SetupGet(x => x.FieldName).Returns("Test");
        valueProviderMock.Setup(x => x.GetValue(It.IsAny<object?>(), "Test")).Returns(12345);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorMock.Object, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be(12345);
    }
}
