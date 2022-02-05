namespace QueryFramework.Abstractions.Tests.CodeGenerationProviders;

public class AbstractionsInterfacesModels : QueryFrameworkCodeGenerationCSharpClassBase
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
        typeof(IQueryCondition),
        typeof(IQueryExpression),
        typeof(IQueryExpressionFunction),
        typeof(IQueryParameter),
        typeof(IQueryParameterValue),
        typeof(IQuerySortOrder)
    };
}
