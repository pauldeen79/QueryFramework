namespace ExpressionFramework.Abstractions;

public interface IConditionEvaluator
{
    bool IsItemValid(object item, ICondition condition);
}
