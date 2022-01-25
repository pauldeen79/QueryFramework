using System;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QueryCombinationExtensions
    {
        /// <summary>
        /// Converts the QueryCombination to SQL.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="ArgumentOutOfRangeException">instance</exception>
        public static string ToSql(this QueryCombination instance)
            => instance switch
            {
                QueryCombination.And => "AND",
                QueryCombination.Or => "OR",
                _ => throw new ArgumentOutOfRangeException(nameof(instance), $"Invalid query combination: {instance}"),
            };
    }
}
