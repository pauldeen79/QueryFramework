﻿namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractBuildersInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => $"{Constants.Namespaces.Abstractions}/Builders";

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetAbstractModels(),
            Constants.Namespaces.Abstractions,
            Constants.Namespaces.AbstractionsBuilders
        )
        .Select
        (
            x => x.ToInterfaceBuilder()
                  .WithPartial()
                  .WithNamespace(Constants.Namespaces.AbstractionsBuilders)
                  .WithName($"I{x.Name}")
                  .Chain(x => x.Methods.RemoveAll(y => y.Name.ToString() == nameof(IValidatableObject.Validate)))
                  .Build()
        )
        .ToArray();
}