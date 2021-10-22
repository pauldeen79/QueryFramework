using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Core.Queries.Builders.Extensions
{
    public static class QueryConditionBuilderExtensions
    {
        public static T WithCombination<T>(this T instance, QueryCombination combination)
            where T : IQueryConditionBuilder
        {
            instance.Combination = combination;
            return instance;
        }

        public static T WithOpenBracket<T>(this T instance, bool openBracket = true)
            where T : IQueryConditionBuilder
        {
            instance.OpenBracket = openBracket;
            return instance;
        }

        public static T WithCloseBracket<T>(this T instance, bool closeBracket = true)
            where T : IQueryConditionBuilder
        {
            instance.CloseBracket = closeBracket;
            return instance;
        }
    }
}
