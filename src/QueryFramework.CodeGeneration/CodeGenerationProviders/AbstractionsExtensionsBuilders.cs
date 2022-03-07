namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

public class AbstractionsExtensionsBuilders : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "QueryFramework.Abstractions\\Extensions";
    public override string DefaultFileName => "Builders.generated.cs";

    protected override string SetMethodNameFormatString => "With{0}";

    public override object CreateModel()
        => GetImmutableBuilderExtensionClasses
        (
            GetModels(),
            "QueryFramework.Core",
            "QueryFramework.Abstractions.Extensions",
            "QueryFramework.Abstractions.Builders"
        );
}
