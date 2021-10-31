namespace QueryFramework.Abstractions.Builders
{
    public interface IQueryConditionBuilder
    {
        bool CloseBracket { get; set; }
        QueryCombination Combination { get; set; }
        IQueryExpressionBuilder Field { get; set; }
        bool OpenBracket { get; set; }
        QueryOperator Operator { get; set; }
        object? Value { get; set; }

        IQueryCondition Build();
    }
}
