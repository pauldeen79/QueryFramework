using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions
{
    public static partial class QueryConditionBuilderExtensions
    {
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
        public static T WithField<T>(this T instance, IQueryExpressionBuilder field)
            where T : IQueryConditionBuilder
        {
            instance.Field = field;
            return instance;
        }
        public static T WithField<T>(this T instance, string field)
            where T : IQueryConditionBuilder
        {
            instance.Field.FieldName = field;
            return instance;
        }
        public static T WithOperator<T>(this T instance, QueryOperator @operator)
            where T : IQueryConditionBuilder
        {
            instance.Operator = @operator;
            return instance;
        }
        public static T WithValue<T>(this T instance, object? value)
            where T : IQueryConditionBuilder
        {
            instance.Value = value;
            return instance;
        }
        public static T WithCombination<T>(this T instance, QueryCombination combination)
            where T : IQueryConditionBuilder
        {
            instance.Combination = combination;
            return instance;
        }
    }
}
