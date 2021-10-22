using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions
{
    public interface ICustomQueryCondition : IQueryCondition
    {
        IQueryConditionBuilder CreateBuilder();
        IQueryCondition With(bool? openBracket, bool? closeBracket, QueryCombination? combination);
    }
}
