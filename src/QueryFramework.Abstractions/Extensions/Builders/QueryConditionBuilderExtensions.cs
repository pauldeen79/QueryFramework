using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions.Builders
{
    public static class QueryConditionBuilderExtensions
    {
        public static T Clear<T>(this T instance)
            where T : IQueryConditionBuilder
        {
            instance.OpenBracket = default;
            instance.CloseBracket = default;
            instance.Field.Clear();
            instance.Operator = default;
            instance.Value = default;
            instance.Combination = default;
            return instance;
        }
        public static T Update<T>(this T instance, IQueryCondition source)
            where T : IQueryConditionBuilder
        {
            instance.OpenBracket = default;
            instance.CloseBracket = default;
            instance.Field.Clear();
            instance.Operator = default;
            instance.Value = default;
            instance.Combination = default;
            if (source != null)
            {
                instance.OpenBracket = source.OpenBracket;
                instance.CloseBracket = source.CloseBracket;
                instance.Field.Update(source.Field);
                instance.Operator = source.Operator;
                instance.Value = source.Value;
                instance.Combination = source.Combination;
            }
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
        public static T WithField<T>(this T instance, IQueryExpressionBuilder field)
            where T : IQueryConditionBuilder
        {
            instance.Field = field;
            return instance;
        }
        public static T WithField<T>(this T instance, IQueryExpression field)
            where T : IQueryConditionBuilder
        {
            instance.Field.Update(field);
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
        public static T WithValue<T>(this T instance, object value)
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
