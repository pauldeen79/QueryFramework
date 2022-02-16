namespace ExpressionFramework.Core.Tests.FunctionEvaluators;

public class SumFunctionEvaluatorTests
{
    [Fact]
    public void TryParse_Return_False_When_Function_Is_Not_Of_Correct_Type()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var functionMock = new Mock<IExpressionFunction>();
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(functionMock.Object, "test", expressionEvaluatorMock.Object, out var _);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Integers()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new[] { 1, 2, 3 };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Nullable_Integers()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new int?[] { 1, 2, null, 3 };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Long_Integers()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new[] { 1L, 2L, 3L };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Nullable_Long_Integers()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new long?[] { 1L, 2L, null, 3L };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Doubles()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new[] { 1d, 2d, 3d };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Nullable_Doubles()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new double?[] { 1d, 2d, null, 3d };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Decimals()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new[] { 1M, 2M, 3M };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Nullable_Decimals()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new decimal?[] { 1M, 2M, null, 3M };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Floats()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new[] { 1f, 2f, 3f };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Of_Correct_Type_With_Nullable_Floats()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new float?[] { 1f, 2f, null, 3f };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(value.Sum());
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Has_Different_Types()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = new object[] { 1, 2L, 3 };
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().BeNull();
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Of_Correct_Type_And_Value_Is_Not_Of_Correct_Type()
    {
        // Arrange
        var sut = new SumFunctionEvaluator();
        var value = 0; //integer, cannot convert this to IEnumerable of ints!
        var function = new SumFunction(new EmptyExpressionBuilder().Build(), null);
        var expressionEvaluatorMock = new Mock<IExpressionEvaluator>();

        // Act
        var actual = sut.TryEvaluate(function, value, expressionEvaluatorMock.Object, out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().BeNull();
    }
}
