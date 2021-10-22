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
        {
            switch (instance)
            {
                case QueryCombination.And:
                    return "AND";
                case QueryCombination.Or:
                    return "OR";
                default:
                    throw new ArgumentOutOfRangeException(nameof(instance), $"Invalid query combination: {instance}");
            }
        }
    }
}
