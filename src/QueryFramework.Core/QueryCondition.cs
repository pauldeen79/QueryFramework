using QueryFramework.Abstractions;

namespace QueryFramework.Core
{
    public sealed record QueryCondition : IQueryCondition
    {
        public QueryCondition(IQueryExpression field,
                              QueryOperator queryOperator,
                              object? value = null,
                              bool openBracket = false,
                              bool closeBracket = false,
                              QueryCombination combination = QueryCombination.And)
        {
            Field = field;
            Operator = queryOperator;
            Value = value;
            OpenBracket = openBracket;
            CloseBracket = closeBracket;
            Combination = combination;
        }

        public QueryCondition(string fieldName,
                              QueryOperator queryOperator,
                              object? value = null,
                              bool openBracket = false,
                              bool closeBracket = false,
                              QueryCombination combination = QueryCombination.And)
        {
            Field = new QueryExpression(fieldName);
            Operator = queryOperator;
            Value = value;
            OpenBracket = openBracket;
            CloseBracket = closeBracket;
            Combination = combination;
        }

        public bool OpenBracket { get; }

        public bool CloseBracket { get; }

        public IQueryExpression Field { get; }

        public QueryOperator Operator { get; }

        public object? Value { get; }

        public QueryCombination Combination { get; }
    }
}
