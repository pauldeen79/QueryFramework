namespace QueryFramework.Abstractions.Tests.Extensions;

public class ExpressionExtensionsTests
{
    [Fact]
    public void TryGetValue_Returns_Correct_Result_On_ConstantExpression_With_Null_Value()
    {
        // Arrange
        var input = new ConstantExpressionBuilder().WithValue(default(object?)).Build();

        // Act
        var result = input.TryGetValue(default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_Returns_Correct_Result_On_ConstantExpression_With_Non_Null_Value()
    {
        // Arrange
        var input = new ConstantExpressionBuilder().WithValue("AA").Build();

        // Act
        var result = input.TryGetValue(default);

        // Assert
        result.Should().Be("AA");
    }

    [Fact]
    public void TryGetValue_Returns_Correct_Result_On_DelegateExpression_With_Null_Value()
    {
        // Arrange
        var input = new DelegateExpressionBuilder().WithValue(_ => default).Build();

        // Act
        var result = input.TryGetValue(default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_Returns_Correct_Result_On_DelegateExpression_With_Non_Null_Value()
    {
        // Arrange
        var input = new DelegateExpressionBuilder().WithValue(_ => "AA").Build();

        // Act
        var result = input.TryGetValue(default);

        // Assert
        result.Should().Be("AA");
    }

    [Fact]
    public void TryGetValue_Returns_Correct_Result_On_ContextExpression_With_Null_Value()
    {
        // Arrange
        var input = new ContextExpressionBuilder().Build();

        // Act
        var result = input.TryGetValue(default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_Returns_Correct_Result_On_ContextExpression_With_Non_Null_Value()
    {
        // Arrange
        var input = new ContextExpressionBuilder().Build();

        // Act
        var result = input.TryGetValue("AA");

        // Assert
        result.Should().Be("AA");
    }

    [Fact]
    public void TryGetValue_Returns_Correct_Result_On_EmptyExpression()
    {
        // Arrange
        var input = new EmptyExpressionBuilder().Build();

        // Act
        var result = input.TryGetValue("context is ignored here");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetFieldName_Throws_On_Unsupported_Expression()
    {
        // Arrange
        var input = new EmptyExpressionBuilder().Build();

        // Act
        input.Invoking(x => x.GetFieldName()).Should().Throw<NotSupportedException>();
    }
}
