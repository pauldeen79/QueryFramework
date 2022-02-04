namespace QueryFramework.InMemory.Tests.FunctionParsers;

public class UpperFunctionParserTests
{
    [Fact]
    public void TryParse_Return_False_When_Function_Is_Not_Correct()
    {
        // Arrange
        var sut = new UpperFunctionParser();
        var functionMock = new Mock<IQueryExpressionFunction>();

        // Act
        var actual = sut.TryParse(functionMock.Object, "test", "TestProperty", out var _);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void TryParse_Returns_True_When_Function_Is_Correct()
    {
        // Arrange
        var sut = new UpperFunctionParser();
        var function = new UpperFunction();

        // Act
        var actual = sut.TryParse(function, "test", "TestProperty", out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be("TEST");
    }

    [Fact]
    public void TryParse_Gives_Empty_String_When_Value_Is_Null()
    {
        // Arrange
        var sut = new UpperFunctionParser();
        var function = new UpperFunction();

        // Act
        var actual = sut.TryParse(function, null, "TestProperty", out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(string.Empty);
    }
}
