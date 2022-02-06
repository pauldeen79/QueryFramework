namespace QueryFramework.FileSystemSearch.Tests;

public class FileSystemDataProviderTests
{
    [Fact]
    public void GetData_Returns_Null_When_Query_Is_Not_FileSystemQuery()
    {
        // Act
        var actual = new FileSystemDataProvider(new Mock<IConditionEvaluator>().Object)
            .GetData<FileData>(new SingleEntityQueryBuilder().Build());

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public void GetData_Returns_Null_When_Data_Is_Not_FileData_Or_LineData()
    {
        // Act
        var actual = new FileSystemDataProvider(new Mock<IConditionEvaluator>().Object)
            .GetData<object>(new FileSystemQuery(Directory.GetCurrentDirectory(), "*.cs", SearchOption.TopDirectoryOnly));

        // Assert
        actual.Should().BeNull();
    }
}
