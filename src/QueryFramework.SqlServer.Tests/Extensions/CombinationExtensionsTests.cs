namespace QueryFramework.SqlServer.Tests.Extensions;

public class CombinationExtensionsTests
{
    [Fact]
    public void ToSql_Throws_On_Invalid_Combination()
    {
        // Arrange
        var combination = (Combination)99;

        // Act & Assert
        Action a = () => combination.ToSql();
        a.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(Combination.And, "AND")]
    [InlineData(Combination.Or, "OR")]
    public void ToSql_Converts_Valid_Combination_Correctly(Combination input, string expectedOutput)
    {
        // Act
        var actual = input.ToSql();

        // Assert
        actual.ShouldBe(expectedOutput);
    }
}
