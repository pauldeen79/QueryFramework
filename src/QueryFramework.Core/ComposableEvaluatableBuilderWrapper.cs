namespace QueryFramework.Core;

public class ComposableEvaluatableBuilderWrapper<T> where T : IQueryBuilder
{
    private readonly T _instance;
    private readonly string _fieldName;

    public ComposableEvaluatableBuilderWrapper(T instance, string fieldName)
    {
        _instance = instance.IsNotNull(nameof(instance));
        _fieldName = fieldName.IsNotNull(nameof(fieldName));
    }

    public T IsEqualTo(object? value)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), value);

    public T IsNull()
        => AddFilterWithOperator(new IsNullOperatorBuilder());

    private T AddFilterWithOperator(OperatorBuilder @operator, object? value)
        => _instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, value));

    private T AddFilterWithOperator(OperatorBuilder @operator)
        => _instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator));
}
