using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QueryProcessorSettingsExtensions
    {
        public static IQueryProcessorSettings WithDefaultTableName(this IQueryProcessorSettings instance, string entityTypeName)
            => string.IsNullOrEmpty(instance.TableName)
                ? new QueryProcessorSettings(entityTypeName, instance.Fields, instance.DefaultOrderBy, instance.DefaultWhere, instance.OverrideLimit, instance.OverrideOffset, instance.ValidateFieldNames)
                : instance;

        public static IQueryProcessorSettings WithPageInfo(this IQueryProcessorSettings instance, int limit, int offset)
            => string.IsNullOrEmpty(instance.TableName)
                ? new QueryProcessorSettings(instance.TableName, instance.Fields, instance.DefaultOrderBy, instance.DefaultWhere, limit, offset, instance.ValidateFieldNames)
                : instance;

    }
}
