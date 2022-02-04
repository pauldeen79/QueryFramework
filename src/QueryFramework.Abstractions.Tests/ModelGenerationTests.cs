namespace QueryFramework.Abstractions.Tests;

public class ModelGenerationTests
{
    private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
    (
        basePath: Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\"),
        generateMultipleFiles: false,
        dryRun: true
    );

    [Fact]
    public void Can_Generate_Records_From_Model()
    {
        Verify(GenerateCode.For<AbstractionsInterfacesModels>(Settings));
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
