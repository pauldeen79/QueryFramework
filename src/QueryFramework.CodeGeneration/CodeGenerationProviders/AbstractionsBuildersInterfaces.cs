namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => $"{Constants.Namespaces.Abstractions}/Builders";

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetCoreModels(),
            Constants.Namespaces.Abstractions,
            Constants.Namespaces.AbstractionsBuilders
        )
        .Select
        (
            x => x.ToInterfaceBuilder()
                  .WithPartial()
                  .WithNamespace(Constants.Namespaces.AbstractionsBuilders)
                  .WithName($"I{x.Name}")
                  .With(x => x.Methods.RemoveAll(y => y.Static))
                  .Build()
        )
        .ToArray();
}
