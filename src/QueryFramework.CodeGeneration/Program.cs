using System.Collections;
using System.ComponentModel;
using System.Reflection;
using ClassFramework.Domain.Builders;
using CsharpExpressionDumper.Abstractions;
using CsharpExpressionDumper.Abstractions.Requests;

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
            .AddSingleton<IObjectHandlerPropertyFilter, SkipDefaultValues>()
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
        var settings = new PipelineSettingsBuilder().WithCopyInterfaces().WithInheritFromInterfaces().WithAllowGenerationWithoutProperties();
        var results = new List<TypeBaseBuilder>();
        foreach (var type in typeof(Program).Assembly.GetTypes().Where(x => x.Namespace?.StartsWith("QueryFramework.CodeGeneration.Models") == true))
        {
            var result = await service.ProcessAsync(new ClassFramework.Pipelines.Reflection.ReflectionContext(type, settings, System.Globalization.CultureInfo.InvariantCulture));
            results.Add(result.Value!.ToBuilder());
        }
        var source = dumper.Dump(results);

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

internal class SkipDefaultValues : IObjectHandlerPropertyFilter
{
    public bool IsValid(ObjectHandlerRequest command, PropertyInfo propertyInfo)
    {
        var defaultValue = GetDefaultValue(propertyInfo);

        var actualValue = propertyInfo.GetValue(command.Instance);

        if (defaultValue is null && actualValue is null)
        {
            return false;
        }

        if (propertyInfo.PropertyType == typeof(string) && actualValue?.Equals(string.Empty) == true)
        {
            return false;
        }

        return defaultValue is null
            || actualValue is null
            || (actualValue is IEnumerable e && !e.OfType<object>().Any())
            || !actualValue.Equals(defaultValue);
    }

    private static object? GetDefaultValue(PropertyInfo propertyInfo)
    {
        var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
        if (defaultValueAttribute is not null)
        {
            return defaultValueAttribute.Value;
        }

        return propertyInfo.PropertyType.IsValueType && Nullable.GetUnderlyingType(propertyInfo.PropertyType) is null
            ? Activator.CreateInstance(propertyInfo.PropertyType)
            : null;
    }
}
