namespace QueryFramework.FileSystemSearch.Tests;

public sealed class IntegrationTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private static readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), @"../../../../");
    private static readonly string _slash = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
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
        actual.ShouldHaveSingleItem();
        actual[0].Directory.ShouldEndWith("FileSystemSearch.Tests");
        actual[0].FileName.ShouldBe("IntegrationTests.cs");
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
        actual.ShouldHaveSingleItem();
        actual[0].Line.ShouldBe($"    public void {nameof(Can_Query_Contents_Using_Linq)}() //*");
    }

    [Fact]
    public void Can_Detect_Block_Scoped_Namespaces_Using_Linq()
    {
        // Act
        var actual = Directory.GetFiles(_basePath, "*.cs", SearchOption.AllDirectories)
            .Select(x => new FileData(x))
            .Where(x => !x.FileName.EndsWith(".generated.cs") && !x.Directory.Contains($"{_slash}bin") && !x.Directory.Contains($"{_slash}obj"))
            .SelectMany(fileData => fileData.Lines.Select((line, lineNumber) => new LineData(line, lineNumber, fileData)))
            .Where(x => x.Line.StartsWith("namespace") && !x.Line.EndsWith(';'))
            .ToArray();

        // Assert
        actual.ShouldBeEmpty(); //we only want to use file-scoped namespaces!
    }

    [Fact]
    public void Can_Query_Metadata_Using_QueryProcessor()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.Directory)).EndsWith("FileSystemSearch.Tests")
            .And(nameof(FileData.FileName)).IsEqualTo("IntegrationTests.cs")
            .BuildTyped());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<FileData>(query);

        // Assert
        actual.ShouldHaveSingleItem();
        actual.First().Directory.ShouldEndWith("FileSystemSearch.Tests");
        actual.First().FileName.ShouldBe("IntegrationTests.cs");
        actual.First().Extension.ShouldBe(".cs");
        actual.First().DateCreated.ShouldNotBe(DateTime.MinValue);
        actual.First().DateLastModified.ShouldNotBe(DateTime.MinValue);
        actual.First().Contents.ShouldNotBeEmpty();
    }

    [Fact]
    public void Can_Query_Contents_Using_QueryProcessor() //*
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.Directory)).EndsWith("FileSystemSearch.Tests")
            .And(nameof(FileData.FileName)).IsEqualTo("IntegrationTests.cs")
            .And(nameof(LineData.Line)).Contains(nameof(Can_Query_Contents_Using_QueryProcessor))
            .And(nameof(LineData.Line)).EndsWith(" //*")
            .BuildTyped());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.ShouldHaveSingleItem();
        actual.First().Line.ShouldBe($"    public void {nameof(Can_Query_Contents_Using_QueryProcessor)}() //*");
        actual.First().LineNumber.ShouldBeGreaterThan(0);
        actual.First().FileData.FileName.ShouldNotBeEmpty();
    }

    [Fact]
    public void Can_Query_Contents_Using_QueryProcessor_On_Both_File_And_Line_Level()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.Directory)).EndsWith("FileSystemSearch.Tests")
            .And(nameof(FileData.FileName)).IsEqualTo("IntegrationTests.cs")
            .And(nameof(FileData.Contents)).Contains("[Fact]")
            .And(nameof(LineData.Line)).Contains(nameof(Can_Query_Contents_Using_QueryProcessor))
            .And(nameof(LineData.Line)).EndsWith(" //*")
            .BuildTyped());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.ShouldHaveSingleItem();
        actual.First().Line.ShouldBe($"    public void {nameof(Can_Query_Contents_Using_QueryProcessor)}() //*");
        actual.First().LineNumber.ShouldBeGreaterThan(0);
        actual.First().FileData.FileName.ShouldNotBeEmpty();
    }

    [Fact]
    public void Can_Detect_Block_Scoped_Namespaces_Using_QueryProcessor()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.FileName)).DoesNotEndWith(".generated.cs")
            .And(nameof(FileData.Directory)).DoesNotContain($"{_slash}bin")
            .And(nameof(FileData.Directory)).DoesNotContain($"{_slash}obj")
            .And(nameof(LineData.Line)).StartsWith("namespace")
            .And(nameof(LineData.Line)).DoesNotEndWith(";")
            .BuildTyped());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.ShouldBeEmpty(); //we only want to use file-scoped namespaces!
    }

    [Fact]
    public void Can_Detect_Usings_Outside_GlobalUsings_Using_QueryProcessor()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(nameof(FileData.FileName)).DoesNotEndWith(".generated.cs")
            .And(nameof(FileData.Directory)).DoesNotContain($"{_slash}bin")
            .And(nameof(FileData.Directory)).DoesNotContain($"{_slash}obj")
            .And(nameof(LineData.Line)).StartsWith("using ")
            .And(nameof(LineData.Line)).EndsWith(";")
            .BuildTyped());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.ShouldBeEmpty(); //we only want to use global usings!
    }

    [Fact]
    public void Result_Is_Empty_When_Conditions_Contain_NonData_Condition_Which_Evaluates_To_False()
    {
        // Arrange
        var query = new FileSystemQuery(_basePath, "*.cs", SearchOption.AllDirectories, new SingleEntityQueryBuilder()
            .Where(new ComposableEvaluatableBuilder().WithLeftExpression(new ConstantExpressionBuilder().WithValue(1))
                                                     .WithOperator(new EqualsOperatorBuilder())
                                                     .WithRightExpression(new ConstantExpressionBuilder().WithValue(2)))
            .BuildTyped());
        var processor = CreateSut();

        // Act
        var actual = processor.FindMany<LineData>(query);

        // Assert
        actual.ShouldBeEmpty(); //filesystem is not read, because condition is false!
    }

    private IQueryProcessor CreateSut() => _serviceProvider.GetRequiredService<IQueryProcessor>();

    public void Dispose() => _serviceProvider.Dispose();
}
