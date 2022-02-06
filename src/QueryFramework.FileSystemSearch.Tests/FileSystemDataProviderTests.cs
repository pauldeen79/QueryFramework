namespace QueryFramework.FileSystemSearch.Tests;

public class FileSystemDataProviderTests
{
    [Fact]
    public void GetData_Returns_Null_When_Query_Is_Not_FileSystemQuery()
    {
        // Act
        var actual = new FileSystemDataProvider(new Mock<IConditionEvaluator>().Object)
            .TryGetData<FileData>(new SingleEntityQueryBuilder().Build(), out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void GetData_Returns_Null_When_Data_Is_Not_FileData_Or_LineData()
    {
        // Act
        var actual = new FileSystemDataProvider(new Mock<IConditionEvaluator>().Object)
            .TryGetData<object>(new FileSystemQuery(Directory.GetCurrentDirectory(), "*.cs", SearchOption.TopDirectoryOnly), out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }
}
