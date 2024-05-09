namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces : QueryFrameworkCSharpClassBase
{
    public AbstractionsInterfaces(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => Constants.Namespaces.Abstractions;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetEntityInterfaces(await GetAbstractionsInterfaces(), Constants.Namespaces.Core, CurrentNamespace);

    protected override bool EnableEntityInheritance => true;
}
