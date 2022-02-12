namespace ExpressionFramework.CodeGeneration.Tests;

public class ModelGenerationTests
{
    private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
    (
        basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
        generateMultipleFiles: false,
        dryRun: true
    );

    [Fact]
    public void Can_Generate_Everything()
    {
        Verify(GenerateCode.For<AbstractionsBuildersInterfaces>(Settings));
        Verify(GenerateCode.For<AbstractionsExtensionsBuilders>(Settings));
        Verify(GenerateCode.For<CoreEntities>(Settings));
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
