namespace QueryFramework.Abstractions.Tests.CodeGenerationProviders;

public class AbstractionsInterfacesModels : CSharpExpressionDumperClassBase
{
    public override string Path => "QueryFramework.CodeGeneration\\CodeGenerationProviders";
    public override string DefaultFileName => "QueryFrameworkCSharpClassBase.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override string[] NamespacesToAbbreviate => new[]
    {
        "System.Collections.Generic",
        "ModelFramework.Objects.Builders",
        "ModelFramework.Objects.Contracts"
    };

    protected override Type[] Models => new[]
    {
        typeof(IQueryParameter),
        typeof(IQueryParameterValue),
        typeof(IQuerySortOrder)
    };

    protected override string Namespace => "QueryFramework.CodeGeneration.CodeGenerationProviders";
    protected override string ClassName => "QueryFrameworkCSharpClassBase";
}
