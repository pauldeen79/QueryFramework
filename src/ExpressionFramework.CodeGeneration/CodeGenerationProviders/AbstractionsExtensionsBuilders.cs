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
            "ExpressionFramework.Abstractions.DomainModel.Extensions",
            "ExpressionFramework.Abstractions.DomainModel.Builders"
        );
}
