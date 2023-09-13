namespace QueryFramework.Abstractions.Extensions;

public static class QueryExtensions
{
    public static T Validate<T>(this T instance)
        where T : IQuery
    {
        if (instance is IValidatableObject validatableQuery)
        {
            var validationResult = validatableQuery.Validate();
            if (!string.IsNullOrEmpty(validationResult))
            {
                throw new ValidationException(validationResult);
            }
        }

        return instance;
    }

    public static string GetTableName(this IQuery instance, string tableName)
        => instance is IDataObjectNameQuery dataObjectNameQuery && !string.IsNullOrEmpty(dataObjectNameQuery.DataObjectName)
            ? dataObjectNameQuery.DataObjectName
            : tableName;
}
