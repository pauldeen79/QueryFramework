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
        Verify(GenerateCode.For<AbstractionsInterfaces>(Settings));
        Verify(GenerateCode.For<AbstractionsBuildersInterfaces>(Settings));
        Verify(GenerateCode.For<AbstractionsExtensionsBuilders>(Settings));
        Verify(GenerateCode.For<CoreRecords>(Settings));
        Verify(GenerateCode.For<CoreBuilders>(Settings));
    }

    private void Verify(GenerateCode generatedCode)
    {
        if (Settings.DryRun)
        {
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }
    }
}
