namespace CodeGeneration.CodeGenerationProviders;

public class CoreEntities : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "QueryFramework.Core";
    public override string DefaultFileName => "Entities.template.generated.cs";

    public override object CreateModel()
        => GetImmutableClasses(GetModels().Where(x => x.Name != "IQueryExpressionFunction").ToArray(), "QueryFramework.Core");
}
