namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractExtensionsBuilders : QueryFrameworkCSharpClassBase
{
    public AbstractionsAbstractExtensionsBuilders(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override string Path => $"{Constants.Namespaces.Abstractions}/Extensions";

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilderExtensions(await GetAbstractionsInterfaces(), Constants.Namespaces.AbstractionsBuilders, Constants.Namespaces.Abstractions, Constants.Namespaces.AbstractionsBuildersExtensions);

    protected override bool EnableEntityInheritance => true;
}
