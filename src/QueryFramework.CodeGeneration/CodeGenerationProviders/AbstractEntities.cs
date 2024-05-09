namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractEntities : QueryFrameworkCSharpClassBase
{
    public AbstractEntities(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => Constants.Namespaces.Core;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetEntities(await GetAbstractModels(), CurrentNamespace);

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.None; // not needed for abstract entities, because each derived class will do its own validation
}
