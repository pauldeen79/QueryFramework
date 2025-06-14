namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractBuildersInterfaces : QueryFrameworkCSharpClassBase
{
    public AbstractionsAbstractBuildersInterfaces(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override string Path => $"{Constants.Namespaces.Abstractions}/Builders";

    public override Task<Result<IEnumerable<TypeBase>>> GetModelAsync(CancellationToken cancellationToken)
        => GetBuilderInterfacesAsync(GetAbstractionsInterfacesAsync(), CurrentNamespace, Constants.Namespaces.Abstractions, Constants.Namespaces.AbstractionsBuilders);

    protected override bool EnableEntityInheritance => true;
}
