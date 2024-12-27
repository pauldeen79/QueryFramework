namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractExtensionsBuilders : QueryFrameworkCSharpClassBase
{
    public AbstractionsAbstractExtensionsBuilders(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => $"{Constants.Namespaces.Abstractions}/Builders/Extensions";

    public override Task<Result<IEnumerable<TypeBase>>> GetModel(CancellationToken cancellationToken)
        => GetBuilderExtensions(GetAbstractionsInterfaces(), Constants.Namespaces.AbstractionsBuilders, Constants.Namespaces.Abstractions, CurrentNamespace);

    protected override bool EnableEntityInheritance => true;
}
