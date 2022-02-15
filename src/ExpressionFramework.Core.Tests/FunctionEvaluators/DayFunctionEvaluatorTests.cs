﻿namespace ExpressionFramework.Core.Tests.FunctionEvaluators;

public class DayFunctionEvaluatorTests
{
    [Fact]
    public void TryParse_Return_False_When_Function_Is_Not_Of_Correct_Type()
    {
        // Arrange
        var sut = new DayFunctionEvaluator();
        var functionMock = new Mock<IExpressionFunction>();
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(functionMock.Object, "test", expressionEvaluatorCallbackMock.Object, out var _);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type()
    {
        // Arrange
        var sut = new DayFunctionEvaluator();
        var value = new DateTime(2020, 2, 3);
        var function = new DayFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorCallbackMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Day);
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Not_Of_Correct_Type()
    {
        // Arrange
        var sut = new DayFunctionEvaluator();
        var value = 0; //integer, cannot convert this to DateTime!
        var function = new DayFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorCallbackMock = new Mock<IExpressionEvaluatorCallback>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorCallbackMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().BeNull();
    }
}