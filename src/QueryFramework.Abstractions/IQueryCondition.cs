namespace QueryFramework.Abstractions
{
    public interface IQueryCondition
    {
        bool OpenBracket { get; }

        bool CloseBracket { get; }

        IQueryExpression Field { get; }

        QueryOperator Operator { get; }

        object Value { get; }

        QueryCombination Combination { get; }
    }
}
