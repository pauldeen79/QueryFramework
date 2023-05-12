namespace QueryFramework.CodeGeneration.CodeGenerationProviders.Queries;

[ExcludeFromCodeCoverage]
public class OverrideBuilders : QueryFrameworkCSharpClassBase
{
    public override string Path => Constants.Paths.QueryBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override IClass? BaseClass => CreateBaseclass(typeof(IQuery), Constants.Namespaces.Core);
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.CoreBuilders;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetOverrideModels(typeof(IQuery)),
            Constants.Namespaces.CoreQueries,
            CurrentNamespace)
        // hacking here... code generation doesn't work out of the box :(
        .Cast<IClass>()
        .Select
        (
            x => new ClassBuilder(x)
                .With(y => { if (y.Interfaces.Count > 0) { y.Interfaces[0] = y.Interfaces[0].Replace("QueryFramework.Core.Queries.", "QueryFramework.Abstractions.Queries.I"); } })
                .Build()
        ).ToArray();
}
