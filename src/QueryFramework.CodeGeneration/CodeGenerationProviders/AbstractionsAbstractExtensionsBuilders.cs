namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractExtensionsBuilders : QueryFrameworkCSharpClassBase
{
    public AbstractionsAbstractExtensionsBuilders(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => $"{Constants.Namespaces.Abstractions}/Builders/Extensions";

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilderExtensions(await GetAbstractionsInterfaces(), Constants.Namespaces.AbstractionsBuilders, Constants.Namespaces.Abstractions, CurrentNamespace);

    protected override bool EnableEntityInheritance => true;
}
