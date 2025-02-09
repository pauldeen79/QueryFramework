﻿using CsharpExpressionDumper.Abstractions;

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

        var dumper = scope.ServiceProvider.GetRequiredService<ICsharpExpressionDumper>();
        var service = scope.ServiceProvider.GetRequiredService<IPipelineService>();
        var settings = new PipelineSettingsBuilder().WithCopyInterfaces().WithInheritFromInterfaces();
        var results = new List<TypeBase>();
        foreach (var type in typeof(Program).Assembly.GetTypes().Where(x => x.Namespace?.StartsWith("QueryFramework.CodeGeneration.Models") == true))
        {
            var result = await service.ProcessAsync(new ClassFramework.Pipelines.Reflection.ReflectionContext(type, settings, System.Globalization.CultureInfo.InvariantCulture));
            results.Add(result.Value!);
        }
        var source = dumper.Dump(results);
        // Generate code
        await Task.WhenAll(generators
            .Select(x => (ICodeGenerationProvider)scope.ServiceProvider.GetRequiredService(x))
            .Select(x => engine.Generate(x, new MultipleStringContentBuilderEnvironment(), new CodeGenerationSettings(basePath, Path.Combine(x.Path, $"{x.GetType().Name}.template.generated.cs")))));

        // Log output to console
        Console.WriteLine($"Code generation completed, check the output in {basePath}");
    }
}
