namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities : QueryFrameworkCSharpClassBase
{
    public CoreEntities(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetEntitiesAsync(GetCoreModelsAsync(), CurrentNamespace);

    public override string Path => Constants.Namespaces.Core;
}
