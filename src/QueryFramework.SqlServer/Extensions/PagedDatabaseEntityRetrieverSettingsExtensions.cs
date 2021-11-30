using CrossCutting.Data.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class PagedDatabaseEntityRetrieverSettingsExtensions
    {
        public static IPagedDatabaseEntityRetrieverSettings WithDefaultTableName(this IPagedDatabaseEntityRetrieverSettings instance, string entityTypeName)
            => string.IsNullOrEmpty(instance.TableName)
                ? new PagedDatabaseEntityRetrieverSettings(entityTypeName, instance.Fields, instance.DefaultOrderBy, instance.DefaultWhere, instance.OverridePageSize)
                : instance;
    }
}
