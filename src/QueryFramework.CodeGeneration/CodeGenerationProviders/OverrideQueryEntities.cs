namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideQueryEntities : QueryFrameworkCSharpClassBase
{
    public OverrideQueryEntities(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => Constants.Paths.Queries;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetEntities(await GetOverrideModels(typeof(Models.IQuery)), CurrentNamespace);

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override async Task<TypeBase?> GetBaseClass() => await CreateBaseClass(typeof(Models.IQuery), Constants.Namespaces.Core);
}
