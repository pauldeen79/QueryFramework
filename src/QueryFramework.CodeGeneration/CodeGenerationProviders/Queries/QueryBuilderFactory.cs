namespace QueryFramework.CodeGeneration.CodeGenerationProviders.Queries;

[ExcludeFromCodeCoverage]
public class QueryBuilderFactory : QueryFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Core}/Builders";

    public override object CreateModel()
        => CreateBuilderFactoryModels(
            GetOverrideModels(typeof(IQuery)),
            new(
                Constants.Namespaces.CoreBuilders,
                nameof(QueryBuilderFactory),
                Constants.TypeNames.Query,
                Constants.Namespaces.CoreBuildersQueries,
                Constants.Types.QueryBuilder,
                Constants.Namespaces.CoreQueries
            )
        );
}
