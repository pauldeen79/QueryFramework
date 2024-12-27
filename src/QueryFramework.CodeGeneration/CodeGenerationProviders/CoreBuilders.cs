namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders : QueryFrameworkCSharpClassBase
{
    public CoreBuilders(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => $"{Constants.Namespaces.Core}/Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetBuilders(GetCoreModels(), CurrentNamespace, Constants.Namespaces.Core);

    protected override bool CreateAsObservable => true;
}
