using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QueryProcessorSettingsExtensions
    {
        public static IQueryProcessorSettings WithDefaultTableName(this IQueryProcessorSettings instance, string entityTypeName)
            => string.IsNullOrEmpty(instance.TableName)
                ? new QueryProcessorSettings(entityTypeName, instance.Fields, instance.DefaultOrderBy, instance.DefaultWhere, instance.OverrideLimit, instance.ValidateFieldNames, instance.InitialParameterNumber)
                : instance;
    }
}
