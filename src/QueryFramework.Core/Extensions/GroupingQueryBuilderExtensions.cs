using System.Linq;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Abstractions.Queries.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class GroupingQueryBuilderExtensions
    {
        public static T GroupBy<T>(this T instance, params IQueryExpressionBuilder[] additionalFieldNames)
            where T : IGroupingQueryBuilder
        {
            instance.GroupByFields.AddRange(additionalFieldNames);
            return instance;
        }

        public static T GroupBy<T>(this T instance, params string[] additionalFieldNames)
            where T : IGroupingQueryBuilder
            => instance.GroupBy(additionalFieldNames.Select(s => new QueryExpressionBuilder().WithFieldName(s)).ToArray());

        public static T Having<T>(this T instance, params IQueryConditionBuilder[] additionalFieldNames)
            where T : IGroupingQueryBuilder
        {
            instance.HavingFields.AddRange(additionalFieldNames);
            return instance;
        }
    }
}
