namespace QueryFramework.CodeGeneration.Tests;

public class ModelGenerationTests
{
    private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
    (
        basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
        generateMultipleFiles: true,
        dryRun: true
    );

    [Fact]
    public void Can_Generate_Everything()
    {
        var multipleContentBuilder = new MultipleContentBuilder(Settings.BasePath);
        GenerateCode.For<AbstractionsInterfaces>(Settings, multipleContentBuilder);
        GenerateCode.For<AbstractionsBuildersInterfaces>(Settings, multipleContentBuilder);
        GenerateCode.For<AbstractionsExtensionsBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<CoreRecords>(Settings, multipleContentBuilder);
        GenerateCode.For<CoreBuilders>(Settings, multipleContentBuilder);
        Verify(multipleContentBuilder);
    }

    private void Verify(MultipleContentBuilder multipleContentBuilder)
    {
        var actual = multipleContentBuilder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().NotBeNullOrEmpty();
    }
}
