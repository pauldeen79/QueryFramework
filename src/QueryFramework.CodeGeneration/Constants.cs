namespace QueryFramework.CodeGeneration;

public static class Constants
{
    public const string ProjectName = "QueryFramework";
    public const string TemplateGenerated = ".template.generated";
    public static class Namespaces
    {
        public const string Abstractions = "QueryFramework.Abstractions";
        public const string AbstractionsBuilders = "QueryFramework.Abstractions.Builders";
        public const string AbstractionsExtensions = "QueryFramework.Abstractions.Extensions";
        public const string AbstractionsDomains = "QueryFramework.Abstractions.Domains";
        public const string Core = "QueryFramework.Core";
        public const string CoreBuilders = "QueryFramework.Core.Builders";

        public const string AbstractionsQueries = "QueryFramework.Abstractions.Queries";
        public const string AbstractionsBuildersQueries = "QueryFramework.Abstractions.Builders.Queries";
        public const string CoreQueries = "QueryFramework.Core.Queries";
        public const string CoreBuildersQueries = "QueryFramework.Core.Builders.Queries";
    }

    public static class Types
    {
        public const string Query = "Query";

        public const string QueryBuilder= "QueryBuilder";
    }

    [ExcludeFromCodeCoverage]
    public static class TypeNames
    {
        public const string IQuery = $"{Namespaces.Abstractions}.IQuery";
        public const string IQueryBuilder = $"{Namespaces.AbstractionsBuilders}.IQueryBuilder";
        public const string Query = $"{Namespaces.Core}.Query";
    }

    [ExcludeFromCodeCoverage]
    public static class Paths
    {
        public const string Queries = $"{Namespaces.Core}/{nameof(Queries)}";

        public const string QueryBuilders = $"{Namespaces.Core}/Builders/{nameof(Queries)}";
    }
}
