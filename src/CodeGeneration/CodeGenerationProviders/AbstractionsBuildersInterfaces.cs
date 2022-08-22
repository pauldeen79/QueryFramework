namespace CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuildersInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "QueryFramework.Abstractions/Builders";
    public override string DefaultFileName => "Interfaces.template.generated.cs";

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetModels(),
            "QueryFramework.Core",
            "QueryFramework.Core.Builders"
        )
        .Select
        (
            x => x.ToInterfaceBuilder()
                  .WithPartial()
                  .WithNamespace("QueryFramework.Abstractions.Builders")
                  .WithName($"I{x.Name}")
                  .With(x => x.Methods.RemoveAll(y => y.Static))
                  .Build()
        )
        .ToArray();
}
