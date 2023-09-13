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
                        y.Interfaces[0] = y.Interfaces[0].Replace("QueryFramework.Core.Queries.", "QueryFramework.Abstractions.Queries.I");
                    }
                    y.Interfaces.Add($"QueryFramework.Abstractions.Builders.Queries.I{y.Name}");
                    y.BaseClass.Replace("QueryFramework.Core.Queries.", "QueryFramework.Abstractions.Queries.I");

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
                // hacking here... doesn't work out of the box :(
                statement.Statement
                    .Replace("new ExpressionFramework.Domain.Builders.ExpressionBuilder(x)", "ExpressionBuilderFactory.Create(x)")
                    .Replace("new QueryFramework.Abstractions.Builders.IQueryParameterBuilder(x)", "new QueryParameterBuilder(x)");
            }
        }
    }

    private static void FixMethods(ClassBuilder y)
    {
        foreach (var method in y.Methods)
        {
            foreach (var statement in method.CodeStatements.OfType<LiteralCodeStatementBuilder>())
            {
                // hacking here... doesn't work out of the box :(
                statement.Statement
                    .Replace("GroupByFilter?.Build()", "GroupByFilter?.BuildTyped()")
                    .Replace("Filter?.Build()", "Filter?.BuildTyped()")
                    .Replace(", OrderByFields", ", OrderByFields.Select(x => x.Build())");
            }
        }

        // hacking here... doesn't work out of the box :(
        y.Methods.Find(z => z.Name.ToString() == "BuildTyped")!.TypeName.Replace("QueryFramework.Core.Queries.", "QueryFramework.Abstractions.Queries.I");
    }
}
