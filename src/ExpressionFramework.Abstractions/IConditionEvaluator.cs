namespace ExpressionFramework.Abstractions;

public interface IConditionEvaluator
{
    bool IsItemValid(object item, IReadOnlyCollection<ICondition> conditions);
}
