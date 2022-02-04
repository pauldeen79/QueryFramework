namespace QueryFramework.InMemory.Tests.FunctionParsers;

public class LeftFunctionParserTests
{
    [Fact]
    public void TryParse_Return_False_When_Function_Is_Not_Correct()
    {
        // Arrange
        var sut = new LeftFunctionParser();
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
        var sut = new LeftFunctionParser();
        var function = new LeftFunction(1);

        // Act
        var actual = sut.TryParse(function, "test", "TestProperty", out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be("t");
    }

    [Fact]
    public void TryParse_Gives_Full_String_When_Length_Is_Too_Long()
    {
        // Arrange
        var sut = new LeftFunctionParser();
        var function = new LeftFunction(5);

        // Act
        var actual = sut.TryParse(function, "test", "TestProperty", out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be("test");
    }

    [Fact]
    public void TryParse_Gives_Full_String_When_Value_Is_Null()
    {
        // Arrange
        var sut = new LeftFunctionParser();
        var function = new LeftFunction(2);

        // Act
        var actual = sut.TryParse(function, null, "TestProperty", out var functionResult);

        // Assert
        actual.Should().BeTrue();
        functionResult.Should().Be(string.Empty);
    }
}
