namespace QueryFramework.CodeGeneration.CodeGenerationProviders.Queries;

[ExcludeFromCodeCoverage]
public class OverrideEntities : QueryFrameworkCSharpClassBase
{
    public override string Path => Constants.Paths.Queries;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override IClass? BaseClass => CreateBaseclass(typeof(IQuery), Constants.Namespaces.Core);

    public override object CreateModel()
        => GetImmutableClasses(GetOverrideModels(typeof(IQuery)), CurrentNamespace)
        // hacking here... code generation doesn't work out of the box :(
        .Cast<IClass>()
        .Select
        (
            x => new ClassBuilder(x)
                .With(y =>
                {
                    if (y.Interfaces.Count > 0)
                    {
                        y.Interfaces[0] = y.Interfaces[0].Replace($"{Constants.Namespaces.CoreQueries}.", $"{Constants.Namespaces.AbstractionsQueries}.I");
                    }
                    if (!y.Name.ToString().EndsWith("Base"))
                    {
                        y.Interfaces.Add($"{Constants.Namespaces.AbstractionsQueries}.I{y.Name}");
                    }
                })
                .BuildTyped()
        )
        .Select(x => FixOverrideEntity(x, "Query", $"{Constants.Namespaces.CoreBuilders}.Queries"))
        .ToArray();
}
