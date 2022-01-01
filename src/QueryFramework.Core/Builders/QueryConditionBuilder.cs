using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Core.Builders
{
    public class QueryConditionBuilder : IQueryConditionBuilder
    {
        public bool OpenBracket { get; set; }
        public bool CloseBracket { get; set; }
        public IQueryExpressionBuilder Field { get; set; }
        public QueryOperator Operator { get; set; }
        public object? Value { get; set; }
        public QueryCombination Combination { get; set; }
        public IQueryCondition Build()
        {
            return new QueryCondition(Field.Build(), Operator, Value, OpenBracket, CloseBracket, Combination);
        }
        public QueryConditionBuilder()
        {
            Field = new QueryExpressionBuilder();
        }
        public QueryConditionBuilder(IQueryCondition source) : this()
        {
            OpenBracket = source.OpenBracket;
            CloseBracket = source.CloseBracket;
            Field.FieldName = source.Field.FieldName;
            Field.Function = source.Field.Function;
            Operator = source.Operator;
            Value = source.Value;
            Combination = source.Combination;
        }
    }
}
