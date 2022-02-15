namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders;

public class CoreBuilders : ExpressionFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ExpressionFramework.Core\\DomainModel\\Builders";

    public override string DefaultFileName => "Builders.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetCoreModels(),
            "ExpressionFramework.Core.DomainModel",
            "ExpressionFramework.Core.DomainModel.Builders",
            "ExpressionFramework.Abstractions.DomainModel.Builders.I{0}"
        )
        .Select
        (
            x => new ClassBuilder(x)
                .Chain(y => y.Methods.RemoveAll(z => z.Static))
                .Chain(y =>
                {
                    if (y.Interfaces[0].EndsWith("ExpressionBuilder"))
                    {
                        y.Interfaces[0] = "ExpressionFramework.Abstractions.DomainModel.Builders.IExpressionBuilder";
                        y.Methods.Single(z => z.Name == "Build").TypeName = "ExpressionFramework.Abstractions.DomainModel.IExpression";
                    }
                })
                .Build()
        )
        .ToArray();
}
