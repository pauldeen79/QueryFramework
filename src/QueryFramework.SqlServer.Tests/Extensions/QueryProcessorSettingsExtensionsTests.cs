namespace QueryFramework.SqlServer.Tests.Extensions;

public class QueryProcessorSettingsExtensionsTests
{
    private const string EntityTypeName = "MyTestEntity";

    [Theory]
    [InlineData(null, EntityTypeName)]
    [InlineData("", EntityTypeName)]
    [InlineData("A", "A")]
    [InlineData("other value", "other value")]
    [InlineData(" ", " ")]
    public void WithDefaultTableName_Returns_Correct_Result(string input, string expectedOutput)
    {
        // Arrange
        var sut = new PagedDatabaseEntityRetrieverSettings(input, "", "", "", null);

        // Act
        var actual = sut.WithDefaultTableName(EntityTypeName);

        // Assert
        actual.TableName.Should().Be(expectedOutput);
    }
}
