namespace QueryFramework.InMemory.Abstractions;

public interface IConditionEvaluator
{
    bool IsItemValid(object item, IReadOnlyCollection<IQueryCondition> conditions);
}
