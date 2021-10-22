using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Builders
{
    public class QueryConditionBuilder : IQueryConditionBuilder
    {
        public bool OpenBracket { get; set; }
        public bool CloseBracket { get; set; }
        public IQueryExpressionBuilder Field { get; set; }
        public QueryOperator Operator { get; set; }
        public object Value { get; set; }
        public QueryCombination Combination { get; set; }
        public IQueryCondition Build()
        {
            return new QueryCondition(Field.Build(), Operator, Value, OpenBracket, CloseBracket, Combination);
        }
        public QueryConditionBuilder(IQueryCondition source = null)
        {
            Field = new QueryExpressionBuilder();
            if (source != null)
            {
                OpenBracket = source.OpenBracket;
                CloseBracket = source.CloseBracket;
                Field.Update(source.Field);
                Operator = source.Operator;
                Value = source.Value;
                Combination = source.Combination;
            }
        }
        public QueryConditionBuilder(IQueryExpression expression,
                                     QueryOperator queryOperator,
                                     object value = null,
                                     bool openBracket = false,
                                     bool closeBracket = false,
                                     QueryCombination combination = QueryCombination.And)
        {
            Field = expression.ToBuilder();
            Operator = queryOperator;
            Value = value;
            OpenBracket = openBracket;
            CloseBracket = closeBracket;
            Combination = combination;
        }
        public QueryConditionBuilder(string fieldName,
                                     QueryOperator queryOperator,
                                     object value = null,
                                     bool openBracket = false,
                                     bool closeBracket = false,
                                     QueryCombination combination = QueryCombination.And)
        {
            Field = new QueryExpressionBuilder(new QueryExpression(fieldName));
            Operator = queryOperator;
            Value = value;
            OpenBracket = openBracket;
            CloseBracket = closeBracket;
            Combination = combination;
        }
    }
}
