namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideQueryBuilders : QueryFrameworkCSharpClassBase
{
    public OverrideQueryBuilders(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => Constants.Paths.QueryBuilders;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilders(await GetOverrideModels(typeof(Models.IQuery)), CurrentNamespace, Constants.Namespaces.CoreQueries);

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool CreateAsObservable => true;
    protected override async Task<TypeBase?> GetBaseClass() => await CreateBaseClass(typeof(Models.IQuery), Constants.Namespaces.Core);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.CoreBuilders;
}
