namespace QueryFramework.FileSystemSearch.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private static string _basePath = Path.Combine(Directory.GetCurrentDirectory(), @"../../../../");
    private static string _slash = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? "\\"
        : "/";

    public IntegrationTests()
        => _serviceProvider = new ServiceCollection()
            .AddQueryFrameworkFileSystemSearch()
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
            .Where(x => !x.FileName.EndsWith(".generated.cs") && !x.Directory.Contains($"{_slash}bin") && !x.Directory.Contains($"{_slash}obj"))
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
        actual.First().Extension.Should().Be(".cs");
        actual.First().DateCreated.Should().BeAfter(DateTime.MinValue);
        actual.First().DateLastModified.Should().BeAfter(DateTime.MinValue);
        actual.First().Contents.Should().NotBeEmpty();
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
        actual.First().LineNumber.Should().BeGreaterThan(0);
        actual.First().FileData.FileName.Should().NotBeEmpty();
    }

    [Fact]
    public void Can_Query_Contents_Using_QueryProcessor_On_Both_File_And_Line_Level()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.Directory).DoesEndWith("FileSystemSearch.Tests"))
            .And(nameof(FileData.FileName).IsEqualTo("IntegrationTests.cs"))
            .And(nameof(FileData.Contents).DoesContain("[Fact]"))
            .And(nameof(LineData.Line).DoesContain(nameof(Can_Query_Contents_Using_QueryProcessor)))
            .And(nameof(LineData.Line).DoesEndWith(" //*"))
            .Build());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.Should().ContainSingle();
        actual.First().Line.Should().Be($"    public void {nameof(Can_Query_Contents_Using_QueryProcessor)}() //*");
        actual.First().LineNumber.Should().BeGreaterThan(0);
        actual.First().FileData.FileName.Should().NotBeEmpty();
    }

    [Fact]
    public void Can_Detect_Block_Scoped_Namespaces_Using_QueryProcessor()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.FileName).DoesNotEndWith(".generated.cs"))
            .And(nameof(FileData.Directory).DoesNotContain($"{_slash}bin"))
            .And(nameof(FileData.Directory).DoesNotContain($"{_slash}obj"))
            .And(nameof(LineData.Line).DoesStartWith("namespace"))
            .And(nameof(LineData.Line).DoesNotEndWith(";"))
            .Build());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.Should().BeEmpty(); //we only want to use file-scoped namespaces!
    }

    [Fact]
    public void Can_Detect_Usings_Outside_GlobalUsings_Using_QueryProcessor()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.FileName).DoesNotEndWith(".generated.cs"))
            .And(nameof(FileData.Directory).DoesNotContain($"{_slash}bin"))
            .And(nameof(FileData.Directory).DoesNotContain($"{_slash}obj"))
            .And(nameof(LineData.Line).DoesStartWith("using "))
            .And(nameof(LineData.Line).DoesEndWith(";"))
            .Build());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.Should().BeEmpty(); //we only want to use global usings!
    }

    [Fact]
    public void Result_Is_Empty_When_Conditions_Contain_NonData_Condition_Which_Evaluates_To_False()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(new ConditionBuilder().WithLeftExpression(new ConstantExpressionBuilder().WithValue(1))
                                         .WithOperator(Operator.Equal)
                                         .WithRightExpression(new ConstantExpressionBuilder().WithValue(2)))
            .Build());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.Should().BeEmpty(); //filesystem is not read, because condition is false!
    }

    private IQueryProcessor CreateSut() => _serviceProvider.GetRequiredService<IQueryProcessor>();

    public void Dispose() => _serviceProvider.Dispose();
}
