namespace ExpressionFramework.Core.Tests.ExpressionEvaluators;

public class FieldExpressionEvaluatorTests
{
    [Fact]
    public void TryEvaluate_Returns_False_When_Expression_Is_Not_A_FieldExpression()
    {
        // Arrange
        var valueProviderMock = new Mock<IValueProvider>();
        var functionEvaluatorMock = new Mock<IFunctionEvaluator>();
        var sut = new FieldExpressionEvaluator(valueProviderMock.Object, new[] { functionEvaluatorMock.Object });
        var expressionMock = new Mock<IExpression>();
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorCallbackMock.Object, out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryEvaluate_Returns_True_When_Expression_Is_A_FieldExpression_Without_Function()
    {
        // Arrange
        var valueProviderMock = new Mock<IValueProvider>();
        var functionEvaluatorMock = new Mock<IFunctionEvaluator>();
        var sut = new FieldExpressionEvaluator(valueProviderMock.Object, new[] { functionEvaluatorMock.Object });
        var expressionMock = new Mock<IFieldExpression>();
        expressionMock.SetupGet(x => x.Function).Returns(default(IExpressionFunction));
        expressionMock.SetupGet(x => x.FieldName).Returns("Test");
        valueProviderMock.Setup(x => x.GetValue(It.IsAny<object?>(), "Test")).Returns(12345);
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorCallbackMock.Object, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be(12345);
    }


    [Fact]
    public void TryEvaluate_Returns_True_When_Expression_Is_A_FieldExpression_With_Function()
    {
        // Arrange
        var valueProviderMock = new Mock<IValueProvider>();
        var functionEvaluatorMock = new Mock<IFunctionEvaluator>();
        var sut = new FieldExpressionEvaluator(valueProviderMock.Object, new[] { functionEvaluatorMock.Object });
        var expressionMock = new Mock<IFieldExpression>();
        var expressionFunctionMock = new Mock<IExpressionFunction>();
        expressionMock.SetupGet(x => x.Function).Returns(expressionFunctionMock.Object);
        expressionMock.SetupGet(x => x.FieldName).Returns("Test");
        object? functionResult = 12345;
        functionEvaluatorMock.Setup(x => x.TryEvaluate(It.IsAny<IExpressionFunction>(), It.IsAny<object?>(), It.IsAny<IExpressionEvaluatorCallback>(), out functionResult)).Returns(true);
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(default, expressionMock.Object, expressionEvaluatorCallbackMock.Object, out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().Be(functionResult);
    }
}
