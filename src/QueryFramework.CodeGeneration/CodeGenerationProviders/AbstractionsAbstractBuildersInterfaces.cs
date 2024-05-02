namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractBuildersInterfaces : QueryFrameworkCSharpClassBase
{
    public AbstractionsAbstractBuildersInterfaces(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override string Path => $"{Constants.Namespaces.Abstractions}/Builders";

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetBuilderInterfaces(await GetAbstractionsInterfaces(), Constants.Namespaces.AbstractionsBuilders, Constants.Namespaces.Abstractions, Constants.Namespaces.AbstractionsBuilders);

    protected override bool EnableEntityInheritance => true;
}
