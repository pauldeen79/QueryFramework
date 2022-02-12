namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders;

public class CoreEntities : ExpressionFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "ExpressionFramework.Core\\DomainModel";

    public override string DefaultFileName => "Entities.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableClasses(GetModels().Where(x => x.Name != "IExpressionFunction").ToArray(),
                               "ExpressionFramework.Core.DomainModel");
}
