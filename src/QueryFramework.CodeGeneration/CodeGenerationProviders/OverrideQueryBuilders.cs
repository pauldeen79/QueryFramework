namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideQueryBuilders : QueryFrameworkCSharpClassBase
{
    public OverrideQueryBuilders(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => Constants.Paths.QueryBuilders;

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetBuilders(GetOverrideModels(typeof(Models.IQuery)), CurrentNamespace, Constants.Namespaces.CoreQueries);

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool CreateAsObservable => true;
    protected override Task<Result<TypeBase>> GetBaseClass() => CreateBaseClass(typeof(Models.IQuery), Constants.Namespaces.Core);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.CoreBuilders;
}
