namespace QueryFramework.SqlServer.Tests.Extensions;

public class OperatorExtensionsTests
{
    [Fact]
    public void ToSql_Throws_On_Invalid_Operator()
    {
        // Act & Assert
        ((Operator)99).Invoking(x => x.ToSql())
                      .Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(Operator.Contains)]
    [InlineData(Operator.EndsWith)]
    [InlineData(Operator.IsNotNull)]
    [InlineData(Operator.IsNotNullOrEmpty)]
    [InlineData(Operator.IsNull)]
    [InlineData(Operator.IsNullOrEmpty)]
    [InlineData(Operator.NotContains)]
    [InlineData(Operator.NotEndsWith)]
    [InlineData(Operator.NotStartsWith)]
    [InlineData(Operator.StartsWith)]
    public void ToSql_Throws_On_Unsupported_Operator(Operator input)
    {
        // Act & Assert
        input.Invoking(x => x.ToSql())
             .Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(Operator.Equal, "=")]
    [InlineData(Operator.GreaterOrEqual, ">=")]
    [InlineData(Operator.Greater, ">")]
    [InlineData(Operator.SmallerOrEqual, "<=")]
    [InlineData(Operator.Smaller, "<")]
    [InlineData(Operator.NotEqual, "<>")]
    public void ToSql_Converts_Valid_Operator_Correctly(Operator input, string expectedOutput)
    {
        // Act
        var actual = input.ToSql();

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData(Operator.NotContains, "NOT ")]
    [InlineData(Operator.NotEndsWith, "NOT ")]
    [InlineData(Operator.NotStartsWith, "NOT ")]
    [InlineData(Operator.Equal, "")]
    [InlineData(Operator.Contains, "")]
    public void ToNot_Returns_Correct_Result(Operator input, string expectedOutput)
    {
        // Act
        var actual = input.ToNot();

        // Assert
        actual.Should().Be(expectedOutput);
    }
}
