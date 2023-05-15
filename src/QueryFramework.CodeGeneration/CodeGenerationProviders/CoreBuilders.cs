namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders : QueryFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Core}/Builders";

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetCoreModels(),
            Constants.Namespaces.Core,
            Constants.Namespaces.CoreBuilders,
            $"{Constants.Namespaces.AbstractionsBuilders}.I{{0}}"
        );
}
