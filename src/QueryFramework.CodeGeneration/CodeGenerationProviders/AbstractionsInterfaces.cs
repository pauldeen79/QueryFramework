namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces : QueryFrameworkCSharpClassBase
{
    public AbstractionsInterfaces(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override string Path => Constants.Namespaces.Abstractions;

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetEntityInterfaces(await GetAbstractionsInterfaces(), Constants.Namespaces.Core, Constants.Namespaces.Abstractions);

    protected override bool EnableEntityInheritance => true;
}
