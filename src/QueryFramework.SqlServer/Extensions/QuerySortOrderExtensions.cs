using System;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QuerySortOrderExtensions
    {
        /// <summary>Converts the specified query sort order to sql.</summary>
        /// <param name="instance">The instance.</param>
        public static string ToSql(this IQuerySortOrder instance)
        {
            switch (instance.Order)
            {
                case QuerySortOrderDirection.Ascending:
                    return "ASC";
                case QuerySortOrderDirection.Descending:
                    return "DESC";
                default:
                    throw new ArgumentOutOfRangeException(nameof(instance), $"Invalid query sort order: {instance}");
            }
        }
    }
}
