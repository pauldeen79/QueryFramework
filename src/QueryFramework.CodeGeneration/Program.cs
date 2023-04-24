namespace QueryFramework.CodeGeneration;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static void Main(string[] args)
    {
        // Setup code generation
        var currentDirectory = Directory.GetCurrentDirectory();
        var basePath = currentDirectory switch
        {
            var x when x.EndsWith(Constants.ProjectName) => Path.Combine(currentDirectory, @"src/"),
            var x when x.EndsWith(Constants.Namespaces.Abstractions) => Path.Combine(currentDirectory, @"../"),
            _ => Path.Combine(currentDirectory, @"../../../../")
        };
        var generateMultipleFiles = true;
        var dryRun = false;
        var multipleContentBuilder = new MultipleContentBuilder { BasePath = basePath };
        var settings = new CodeGenerationSettings(basePath, generateMultipleFiles, false, dryRun);

        // Generate code
        var generationTypeNames = new[] { "Entities", "Builders" };
        var generators = typeof(QueryFrameworkCSharpClassBase).Assembly.GetExportedTypes().Where(x => x.BaseType == typeof(QueryFrameworkCSharpClassBase)).ToArray();
        var generationTypes = generators.Where(x => x.Name.EndsWithAny(generationTypeNames));
        var scaffoldingTypes = generators.Where(x => !x.Name.EndsWithAny(generationTypeNames));
        _ = generationTypes.Select(x => (QueryFrameworkCSharpClassBase)Activator.CreateInstance(x)!).Select(x => GenerateCode.For(settings.ForGeneration(), multipleContentBuilder, x)).ToArray();
        _ = scaffoldingTypes.Select(x => (QueryFrameworkCSharpClassBase)Activator.CreateInstance(x)!).Select(x => GenerateCode.For(settings.ForScaffolding(), multipleContentBuilder, x)).ToArray();

        // Log output to console
        if (string.IsNullOrEmpty(basePath))
        {
            Console.WriteLine(multipleContentBuilder.ToString());
        }
        else
        {
            Console.WriteLine($"Code generation completed, check the output in {basePath}");
            Console.WriteLine($"Generated files: {multipleContentBuilder.Contents.Count()}");
            foreach (var content in multipleContentBuilder.Contents)
            {
                Console.WriteLine(content.FileName);
            }
        }
    }
}
