namespace QueryFramework.FileSystemSearch.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private static string _basePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\");

    public IntegrationTests()
        => _serviceProvider = new ServiceCollection()
            .AddFileSystemSearch()
            .BuildServiceProvider();

    [Fact]
    public void Can_Query_Metadata_Using_Linq()
    {
        // Act
        var actual = Directory.GetFiles(_basePath, "*.cs", SearchOption.AllDirectories)
            .Select(x => new FileData(x))
            .Where(x => x.Directory.EndsWith("FileSystemSearch.Tests") && x.FileName == "IntegrationTests.cs")
            .ToArray();

        // Assert
        actual.Should().ContainSingle();
        actual.First().Directory.Should().EndWith("FileSystemSearch.Tests");
        actual.First().FileName.Should().Be("IntegrationTests.cs");
    }

    [Fact]
    public void Can_Query_Contents_Using_Linq() //*
    {
        // Act
        var actual = Directory.GetFiles(_basePath, "*.cs", SearchOption.AllDirectories)
            .Select(x => new FileData(x))
            .Where(x => x.Directory.EndsWith("FileSystemSearch.Tests") && x.FileName == "IntegrationTests.cs")
            .SelectMany(fileData => fileData.Lines.Select((line, lineNumber) => new LineData(line, lineNumber, fileData)))
            .Where(x => x.Line.Contains(nameof(Can_Query_Contents_Using_Linq)) && x.Line.EndsWith(" //*"))
            .ToArray();

        // Assert
        actual.Should().ContainSingle();
        actual.First().Line.Should().Be($"    public void {nameof(Can_Query_Contents_Using_Linq)}() //*");
    }

    [Fact]
    public void Can_Detect_Block_Scoped_Namespaces_Using_Linq()
    {
        // Act
        var actual = Directory.GetFiles(_basePath, "*.cs", SearchOption.AllDirectories)
            .Select(x => new FileData(x))
            .Where(x => !x.FileName.EndsWith(".generated.cs"))
            .SelectMany(fileData => fileData.Lines.Select((line, lineNumber) => new LineData(line, lineNumber, fileData)))
            .Where(x => x.Line.StartsWith("namespace") && !x.Line.EndsWith(";"))
            .ToArray();

        // Assert
        actual.Should().BeEmpty(); //we only want to use file-scoped namespaces!
    }

    [Fact]
    public void Can_Query_Metadata_Using_QueryProcessor()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.Directory).DoesEndWith("FileSystemSearch.Tests"))
            .And(nameof(FileData.FileName).IsEqualTo("IntegrationTests.cs"))
            .Build());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<FileData>(query);

        // Assert
        actual.Should().ContainSingle();
        actual.First().Directory.Should().EndWith("FileSystemSearch.Tests");
        actual.First().FileName.Should().Be("IntegrationTests.cs");
    }

    [Fact]
    public void Can_Query_Contents_Using_QueryProcessor() //*
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.Directory).DoesEndWith("FileSystemSearch.Tests"))
            .And(nameof(FileData.FileName).IsEqualTo("IntegrationTests.cs"))
            .And(nameof(LineData.Line).DoesContain(nameof(Can_Query_Contents_Using_QueryProcessor)))
            .And(nameof(LineData.Line).DoesEndWith(" //*"))
            .Build());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.Should().ContainSingle();
        actual.First().Line.Should().Be($"    public void {nameof(Can_Query_Contents_Using_QueryProcessor)}() //*");
    }

    [Fact]
    public void Can_Detect_Block_Scoped_Namespaces_Using_QueryProcessor()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.FileName).DoesNotEndWith(".generated.cs"))
            .And(nameof(LineData.Line).DoesStartWith("namespace"))
            .And(nameof(LineData.Line).DoesNotEndWith(";"))
            .Build());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.Should().BeEmpty(); //we only want to use file-scoped namespaces!
    }

    private IQueryProcessor CreateSut() => _serviceProvider.GetRequiredService<IQueryProcessor>();

    public void Dispose() => _serviceProvider.Dispose();
}
