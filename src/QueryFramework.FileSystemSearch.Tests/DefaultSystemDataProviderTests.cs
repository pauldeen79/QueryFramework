﻿namespace QueryFramework.FileSystemSearch.Tests;

public class DefaultSystemDataProviderTests
{
    [Fact]
    public void GetData_Returns_Null_When_Query_Is_Not_FileSystemQuery()
    {
        // Act
        var actual = new DefaultFileDataProvider(new Mock<IExpressionEvaluator>().Object, new Mock<IFileDataProvider>().Object)
            .TryGetData<FileData>(new SingleEntityQuery(), out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void GetData_Returns_Null_When_Data_Is_Not_FileData_Or_LineData()
    {
        // Act
        var actual = new DefaultFileDataProvider(new Mock<IExpressionEvaluator>().Object, new Mock<IFileDataProvider>().Object)
            .TryGetData<object>(new FileSystemQuery(Directory.GetCurrentDirectory(), "*.cs", SearchOption.TopDirectoryOnly), out var result);

        // Assert
        actual.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void GetData_Returns_Valid_FileData_Result_When_Query_Is_FileSystemQuery()
    {
        // Arrange
        var fileDataProviderMock = new Mock<IFileDataProvider>();
        var data = new[] { new Mock<IFileData>().Object }.AsEnumerable();
        fileDataProviderMock.Setup(x => x.Get(It.IsAny<IFileSystemQuery>())).Returns(data);

        // Act
        var actual = new DefaultFileDataProvider(new Mock<IExpressionEvaluator>().Object, fileDataProviderMock.Object)
            .TryGetData<IFileData>(new FileSystemQuery(Directory.GetCurrentDirectory(), "*.cs", SearchOption.TopDirectoryOnly), out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void GetData_Returns_Valid_LineData_Result_When_Query_Is_FileSystemQuery()
    {
        // Arrange
        var fileDataProviderMock = new Mock<IFileDataProvider>();
        var fileDataMock = new Mock<IFileData>();
        var lines = new[] { "1", "2", "3" };
        fileDataMock.SetupGet(x => x.Lines).Returns(lines);
        var fileData = new[] { fileDataMock.Object }.AsEnumerable();
        fileDataProviderMock.Setup(x => x.Get(It.IsAny<IFileSystemQuery>())).Returns(fileData);

        // Act
        var actual = new DefaultFileDataProvider(new Mock<IExpressionEvaluator>().Object, fileDataProviderMock.Object)
            .TryGetData<ILineData>(new FileSystemQuery(Directory.GetCurrentDirectory(), "*.cs", SearchOption.TopDirectoryOnly), out var result);

        // Assert
        actual.Should().BeTrue();
        result.Should().NotBeNull();
        result?.Select(x => x.Line).Should().BeEquivalentTo(lines);
    }
}