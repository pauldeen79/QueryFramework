using System.ComponentModel.DataAnnotations;
using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Abstractions.Extensions.Queries
{
    public static class SingleEntityQueryExtensions
    {
        public static T Validate<T>(this T instance)
            where T : ISingleEntityQuery
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

        public static string GetTableName(this ISingleEntityQuery instance, string tableName)
            => instance is IDataObjectNameQuery asdoq && !string.IsNullOrEmpty(asdoq.DataObjectName)
                ? asdoq.DataObjectName
                : tableName;
    }
}
