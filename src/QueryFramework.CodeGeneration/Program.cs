namespace QueryFramework.CodeGeneration;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static async Task Main(string[] args)
    {
        // Setup code generation
        var currentDirectory = Directory.GetCurrentDirectory();
        var basePath = currentDirectory.EndsWith("QueryFramework")
            ? Path.Combine(currentDirectory, @"src/")
            : Path.Combine(currentDirectory, @"../../../../");
        var services = new ServiceCollection()
            .AddParsers()
            .AddClassFrameworkPipelines()
            .AddTemplateFramework()
            .AddTemplateFrameworkChildTemplateProvider()
            .AddTemplateFrameworkCodeGeneration()
            .AddTemplateFrameworkRuntime()
            .AddCsharpExpressionDumper()
            .AddClassFrameworkTemplates()
            .AddScoped<IAssemblyInfoContextService, MyAssemblyInfoContextService>();

        var generators = typeof(Program).Assembly.GetExportedTypes()
            .Where(x => !x.IsAbstract && x.BaseType == typeof(QueryFrameworkCSharpClassBase))
            .ToArray();

        foreach (var type in generators)
        {
            services.AddScoped(type);
        }

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var engine = scope.ServiceProvider.GetRequiredService<ICodeGenerationEngine>();

        // Generate code
        foreach (var generatorType in generators)
        {
            var generator = (CsharpClassGeneratorCodeGenerationProviderBase)scope.ServiceProvider.GetRequiredService(generatorType);
            var result = await engine.Generate(generator, new MultipleStringContentBuilderEnvironment(), new CodeGenerationSettings(basePath, Path.Combine(generator.Path, $"{generatorType.Name}.template.generated.cs"))).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                Console.WriteLine("Errors:");
                WriteError(result);
                break;
            }
        }

        // Log output to console
        Console.WriteLine($"Code generation completed, check the output in {basePath}");
    }

    private static void WriteError(Result error)
    {
        Console.WriteLine($"{error.Status} {error.ErrorMessage}");
        foreach (var validationError in error.ValidationErrors)
        {
            Console.WriteLine($"{string.Join(",", validationError.MemberNames)}: {validationError.ErrorMessage}");
        }

        foreach (var innerResult in error.InnerResults)
        {
            WriteError(innerResult);
        }
    }
}
