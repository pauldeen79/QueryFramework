namespace QueryFramework.FileSystemSearch.Tests;

public class DefaultSystemDataProviderTests
{
    [Fact]
    public void GetData_Returns_Null_When_Query_Is_Not_FileSystemQuery()
    {
        // Act
        var actual = new DefaultFileDataProvider(Substitute.For<IFileDataProvider>())
            .TryGetData<FileData>(new SingleEntityQuery(), out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void GetData_Returns_Null_When_Data_Is_Not_FileData_Or_LineData()
    {
        // Act
        var actual = new DefaultFileDataProvider(Substitute.For<IFileDataProvider>())
            .TryGetData<object>(new FileSystemQuery(Directory.GetCurrentDirectory(), "*.cs", SearchOption.TopDirectoryOnly), out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void GetData_Returns_Valid_FileData_Result_When_Query_Is_FileSystemQuery()
    {
        // Arrange
        var fileDataProviderMock = Substitute.For<IFileDataProvider>();
        var data = new[] { Substitute.For<IFileData>() }.AsEnumerable();
        fileDataProviderMock.Get(Arg.Any<IFileSystemQuery>()).Returns(data);

        // Act
        var actual = new DefaultFileDataProvider(fileDataProviderMock)
            .TryGetData<IFileData>(new FileSystemQuery(Directory.GetCurrentDirectory(), "*.cs", SearchOption.TopDirectoryOnly), out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void GetData_Returns_Valid_LineData_Result_When_Query_Is_FileSystemQuery()
    {
        // Arrange
        var fileDataProviderMock = Substitute.For<IFileDataProvider>();
        var fileDataMock = Substitute.For<IFileData>();
        var lines = new[] { "1", "2", "3" };
        fileDataMock.Lines.Returns(lines);
        var fileData = new[] { fileDataMock }.AsEnumerable();
        fileDataProviderMock.Get(Arg.Any<IFileSystemQuery>()).Returns(fileData);

        // Act
        var actual = new DefaultFileDataProvider(fileDataProviderMock)
            .TryGetData<ILineData>(new FileSystemQuery(Directory.GetCurrentDirectory(), "*.cs", SearchOption.TopDirectoryOnly), out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().NotBeNull();
        result?.Select(x => x.Line).Should().BeEquivalentTo(lines);
    }
}
