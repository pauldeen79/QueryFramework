namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideQueryEntities : QueryFrameworkCSharpClassBase
{
    public OverrideQueryEntities(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => Constants.Paths.Queries;

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetEntities(GetOverrideModels(typeof(Models.IQuery)), CurrentNamespace);

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override Task<Result<TypeBase>> GetBaseClass() => CreateBaseClass(typeof(Models.IQuery), Constants.Namespaces.Core);
}
