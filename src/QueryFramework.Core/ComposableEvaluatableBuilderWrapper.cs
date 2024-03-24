namespace QueryFramework.Core;

public class ComposableEvaluatableBuilderWrapper<T> where T : IQueryBuilder
{
    private readonly T _instance;
    private readonly string _fieldName;
    private readonly Combination? _combination;
    private bool _startGroup;
    private bool _endGroup;

    public ComposableEvaluatableBuilderWrapper(T instance, string fieldName, Combination? combination = null)
    {
        _instance = instance.IsNotNull(nameof(instance));
        _fieldName = fieldName.IsNotNull(nameof(fieldName));
        _combination = combination;
    }

    public T IsEqualTo(object? value)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), value);

    public T IsNull()
        => AddFilterWithOperator(new IsNullOperatorBuilder());

    private T AddFilterWithOperator(OperatorBuilder @operator, object? value)
        => _instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, value, _combination, _startGroup, _endGroup));

    private T AddFilterWithOperator(OperatorBuilder @operator)
        => _instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, _combination, _startGroup, _endGroup));

    public ComposableEvaluatableBuilderWrapper<T> WithStartGroup(bool startGroup = true)
    {
        _startGroup = startGroup;
        return this;
    }

    public ComposableEvaluatableBuilderWrapper<T> WithEndGroup(bool endGroup = true)
    {
        _endGroup = endGroup;
        return this;
    }
}
