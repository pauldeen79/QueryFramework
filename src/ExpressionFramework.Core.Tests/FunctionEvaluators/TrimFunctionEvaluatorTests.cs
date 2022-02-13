﻿namespace ExpressionFramework.Core.Tests.FunctionEvaluators;

public class TrimFunctionEvaluatorTests
{
    [Fact]
    public void TryParse_Return_False_When_Function_Is_Not_Correct()
    {
        // Arrange
        var sut = new TrimFunctionEvaluator();
        var functionMock = new Mock<IExpressionFunction>();

        // Act
        var actual = sut.TryEvaluate(functionMock.Object, "test", "TestProperty", out var _);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Correct()
    {
        // Arrange
        var sut = new TrimFunctionEvaluator();
        var function = new TrimFunction();

        // Act
        var actual = sut.TryEvaluate(function, "  test  ", "TestProperty", out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be("test");
    }

    [Fact]
    public void TryParse_Gives_Empty_String_When_Value_Is_Null()
    {
        // Arrange
        var sut = new TrimFunctionEvaluator();
        var function = new TrimFunction();

        // Act
        var actual = sut.TryEvaluate(function, null, "TestProperty", out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(string.Empty);
    }
}
