namespace QueryFramework.Abstractions.Builders
{
    public interface IQueryConditionBuilder
    {
        bool OpenBracket { get; set; }
        bool CloseBracket { get; set; }
        IQueryExpressionBuilder Field { get; set; }
        QueryOperator Operator { get; set; }
        object? Value { get; set; }
        QueryCombination Combination { get; set; }

        IQueryCondition Build();
    }
}
