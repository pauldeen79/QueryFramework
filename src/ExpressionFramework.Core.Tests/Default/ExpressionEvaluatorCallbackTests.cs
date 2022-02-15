﻿namespace ExpressionFramework.Core.Tests.Default;

public class ExpressionEvaluatorCallbackTests
{
    private readonly ExpressionEvaluatorMock _expressionEvaluatorMock = new ExpressionEvaluatorMock();
    private readonly FunctionEvaluatorMock _functionEvaluatorMock = new FunctionEvaluatorMock();

    [Fact]
    public void Evaluate_Throws_When_Expression_Is_Not_Supported()
    {
        // Arrange
        var sut = CreateSut();
        var expression = new Mock<IExpression>().Object;

        // Act
        sut.Invoking(x => x.Evaluate(null, expression))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .WithParameterName("expression")
           .And.Message.Should().StartWith("Unsupported expression: [IExpressionProxy]");
    }

    [Fact]
    public void Evaluate_Throws_When_Function_Is_Not_Supported()
    {
        // Arrange
        var sut = CreateSut();
        var functionMock = new Mock<IExpressionFunction>();
        var expressionMock = new Mock<IExpression>();
        expressionMock.SetupGet(x => x.Function).Returns(functionMock.Object);
        var expression = expressionMock.Object;
        _expressionEvaluatorMock.Delegate = new Func<object?, IExpression, IExpressionEvaluatorCallback, Tuple<bool, object?>>((_, _, _) => new Tuple<bool, object?>(true, null));

        // Act
        sut.Invoking(x => x.Evaluate(null, expression))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .WithParameterName("expression")
           .And.Message.Should().StartWith("Unsupported function: [IExpressionFunctionProxy]");
    }

    [Fact]
    public void Evaluate_Returns_Correct_Value_Without_Function()
    {
        // Arrange
        var sut = CreateSut();
        var expression = new Mock<IExpression>().Object;
        _expressionEvaluatorMock.Delegate = new Func<object?, IExpression, IExpressionEvaluatorCallback, Tuple<bool, object?>>((_, _, _) => new Tuple<bool, object?>(true, 12345));

        // Act
        var actual = sut.Evaluate(null, expression);

        // Assert
        actual.Should().Be(12345);
    }

    [Fact]
    public void Evaluate_Returns_Correct_Value_With_Function()
    {
        // Arrange
        var sut = CreateSut();
        var functionMock = new Mock<IExpressionFunction>();
        var expressionMock = new Mock<IExpression>();
        expressionMock.SetupGet(x => x.Function).Returns(functionMock.Object);
        var expression = expressionMock.Object;
        _expressionEvaluatorMock.Delegate = new Func<object?, IExpression, IExpressionEvaluatorCallback, Tuple<bool, object?>>((_, _, _) => new Tuple<bool, object?>(true, 12345));
        _functionEvaluatorMock.Delegate = new Func<IExpressionFunction, object?, IExpressionEvaluatorCallback, Tuple<bool, object?>>((_, value, _) => new Tuple<bool, object?>(true, Convert.ToInt32(value) * 2));

        // Act
        var actual = sut.Evaluate(null, expression);

        // Assert
        actual.Should().Be(12345 * 2);
    }

    private ExpressionEvaluatorCallback CreateSut()
        => new ExpressionEvaluatorCallback(new[] { _expressionEvaluatorMock },
                                           new[] { _functionEvaluatorMock });
}
