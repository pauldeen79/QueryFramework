namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders;

public class CoreBuilders : ExpressionFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ExpressionFramework.Core\\DomainModel\\Builders";

    public override string DefaultFileName => "Builders.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetModels().Where(x => x.Name != "IExpressionFunction").ToArray(),
            "ExpressionFramework.Core.DomainModel",
            "ExpressionFramework.Core.DomainModel.Builders",
            "ExpressionFramework.Abstractions.DomainModel.Builders.I{0}"
        )
        .Select(x => new ClassBuilder(x).Chain(y => y.Methods.RemoveAll(z => z.Static)).Build())
        .ToArray();
}
