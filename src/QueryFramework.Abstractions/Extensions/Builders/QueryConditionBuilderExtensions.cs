using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions.Builders
{
    public static class QueryConditionBuilderExtensions
    {
        public static IQueryConditionBuilder Clear(this IQueryConditionBuilder instance)
        {
            instance.OpenBracket = default;
            instance.CloseBracket = default;
            instance.Field.Clear();
            instance.Operator = default;
            instance.Value = default;
            instance.Combination = default;
            return instance;
        }
        public static IQueryConditionBuilder Update(this IQueryConditionBuilder instance, IQueryCondition source)
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
        public static IQueryConditionBuilder WithOpenBracket(this IQueryConditionBuilder instance, bool openBracket)
        {
            instance.OpenBracket = openBracket;
            return instance;
        }
        public static IQueryConditionBuilder WithCloseBracket(this IQueryConditionBuilder instance, bool closeBracket)
        {
            instance.CloseBracket = closeBracket;
            return instance;
        }
        public static IQueryConditionBuilder WithField(this IQueryConditionBuilder instance, IQueryExpressionBuilder field)
        {
            instance.Field = field;
            return instance;
        }
        public static IQueryConditionBuilder WithField(this IQueryConditionBuilder instance, IQueryExpression field)
        {
            instance.Field.Update(field);
            return instance;
        }
        public static IQueryConditionBuilder WithField(this IQueryConditionBuilder instance, string field)
        {
            instance.Field.FieldName = field;
            return instance;
        }
        public static IQueryConditionBuilder WithOperator(this IQueryConditionBuilder instance, QueryOperator @operator)
        {
            instance.Operator = @operator;
            return instance;
        }
        public static IQueryConditionBuilder WithValue(this IQueryConditionBuilder instance, object value)
        {
            instance.Value = value;
            return instance;
        }
        public static IQueryConditionBuilder WithCombination(this IQueryConditionBuilder instance, QueryCombination combination)
        {
            instance.Combination = combination;
            return instance;
        }
    }
}
