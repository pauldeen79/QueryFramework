using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;

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
