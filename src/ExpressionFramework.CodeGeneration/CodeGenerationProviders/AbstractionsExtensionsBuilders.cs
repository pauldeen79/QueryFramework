namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders;

public class AbstractionsExtensionsBuilders : ExpressionFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ExpressionFramework.Abstractions\\DomainModel\\Extensions";

    public override string DefaultFileName => "Builders.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override string SetMethodNameFormatString => "With{0}";

    public override object CreateModel()
        => GetImmutableBuilderExtensionClasses
        (
            GetBaseModels(),
            "ExpressionFramework.Core.DomainModel",
            "ExpressionFramework.Core.DomainModel.Builders"
        )
        .Select
        (
            x => new ClassBuilder(x)
                  .WithNamespace("ExpressionFramework.Abstractions.DomainModel.Extensions")
                  .Chain
                  (
                    x => x.Methods.ForEach(y => y.AddGenericTypeArguments("T").AddGenericTypeArgumentConstraints($"where T : ExpressionFramework.Abstractions.DomainModel.Builders.I{y.TypeName}"))
                  )
                  .Chain
                  (
                    x => x.Methods.ForEach(y => y.WithTypeName("T")
                                                 .Chain(z => z.Parameters.First().WithTypeName(z.TypeName)))
                  )
                  .Build()
        )
        .ToArray();
}
