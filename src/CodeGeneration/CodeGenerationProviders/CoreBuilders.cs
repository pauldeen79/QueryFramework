namespace CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "QueryFramework.Core/Builders";
    public override string DefaultFileName => "Builders.template.generated.cs";

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetModels().Where(x => x.Name != "IQueryExpressionFunction").ToArray(),
            "QueryFramework.Core",
            "QueryFramework.Core.Builders",
            "QueryFramework.Abstractions.Builders.I{0}"
        )
        .Select(x => new ClassBuilder(x).Chain(y => y.Methods.RemoveAll(z => z.Static)).Build())
        .ToArray();
}
