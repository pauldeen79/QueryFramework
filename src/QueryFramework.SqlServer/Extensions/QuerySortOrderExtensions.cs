using System;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QuerySortOrderExtensions
    {
        /// <summary>Converts the specified query sort order to sql.</summary>
        /// <param name="instance">The instance.</param>
        public static string ToSql(this IQuerySortOrder instance)
            => instance.Order switch
            {
                QuerySortOrderDirection.Ascending => "ASC",
                QuerySortOrderDirection.Descending => "DESC",
                _ => throw new ArgumentOutOfRangeException(nameof(instance), $"Invalid query sort order: {instance}"),
            };
    }
}
