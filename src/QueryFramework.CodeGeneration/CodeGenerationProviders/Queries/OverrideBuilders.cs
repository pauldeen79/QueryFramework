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
            base.CurrentNamespace)
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
                    y.Interfaces.Add($"{Constants.Namespaces.AbstractionsBuildersQueries}.I{y.Name}");
                    y.WithBaseClass(y.BaseClass.Replace($"{Constants.Namespaces.CoreQueries}.", $"{Constants.Namespaces.AbstractionsQueries}.I"));

                    FixMethods(y);
                    FixConstructors(y);
                })
                .Build()
        ).ToArray();

    private static void FixConstructors(ClassBuilder y)
    {
        foreach (var ctor in y.Constructors)
        {
            foreach (var statement in ctor.CodeStatements.OfType<LiteralCodeStatementBuilder>())
            {
                statement.WithStatement(statement.Statement
                    .Replace("new ExpressionFramework.Domain.Builders.ExpressionBuilder(x)", "ExpressionBuilderFactory.Create(x)")
                    .Replace($"new {Constants.Namespaces.AbstractionsBuilders}.I", $"new {Constants.Namespaces.CoreBuilders}."));
            }
        }
    }

    private static void FixMethods(ClassBuilder y)
    {
        foreach (var method in y.Methods)
        {
            foreach (var statement in method.CodeStatements.OfType<LiteralCodeStatementBuilder>())
            {
                statement.WithStatement(statement.Statement
                    .Replace("GroupByFilter?.Build()", "GroupByFilter?.BuildTyped()")
                    .Replace("Filter?.Build()", "Filter?.BuildTyped()")
                    /*.Replace(", OrderByFields", ", OrderByFields.Select(x => x.Build())")*/);
            }
        }

        var z = y.Methods.Find(z => z.Name.ToString() == "BuildTyped");
        z!.WithTypeName(z.TypeName.Replace($"{Constants.Namespaces.CoreQueries}.", $"{Constants.Namespaces.AbstractionsQueries}.I"));
    }
}
