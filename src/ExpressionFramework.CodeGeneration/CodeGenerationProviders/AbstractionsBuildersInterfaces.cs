namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders;

public class AbstractionsBuildersInterfaces : ExpressionFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ExpressionFramework.Abstractions\\DomainModel\\Builders";

    public override string DefaultFileName => "Interfaces.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetBaseModels(),
            "ExpressionFramework.Core.DomainModel",
            "ExpressionFramework.Core.DomainModel.Builders"
        )
        .Select
        (
            x => x.ToInterfaceBuilder()
                  .WithPartial()
                  .WithNamespace("ExpressionFramework.Abstractions.DomainModel.Builders")
                  .WithName($"I{x.Name}")
                  .Chain(x => x.Methods.RemoveAll(y => y.Static))
                  .Build()
        )
        .ToArray();
}
