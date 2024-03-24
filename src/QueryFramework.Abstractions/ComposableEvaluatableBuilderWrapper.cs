namespace QueryFramework.Abstractions;

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

    public T Contains(object? value)
        => AddFilterWithOperator(new StringContainsOperatorBuilder(), value);

    public T EndsWith(object? value)
        => AddFilterWithOperator(new EndsWithOperatorBuilder(), value);

    public T IsEqualTo(object? value)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), value);

    public T IsGreaterOrEqualThan(object? value)
        => AddFilterWithOperator(new IsGreaterOrEqualOperatorBuilder(), value);

    public T IsGreaterThan(object? value)
        => AddFilterWithOperator(new IsGreaterOperatorBuilder(), value);

    public T IsNotNull()
        => AddFilterWithOperator(new IsNotNullOperatorBuilder());

    public T IsNotNullOrEmpty()
        => AddFilterWithOperator(new IsNotNullOrEmptyOperatorBuilder());

    public T IsNotNullOrWhiteSpace()
        => AddFilterWithOperator(new IsNotNullOrWhiteSpaceOperatorBuilder());

    public T IsNull()
        => AddFilterWithOperator(new IsNullOperatorBuilder());

    public T IsNullOrEmpty()
        => AddFilterWithOperator(new IsNullOrEmptyOperatorBuilder());

    public T IsNullOrWhiteSpace()
        => AddFilterWithOperator(new IsNullOrWhiteSpaceOperatorBuilder());

    public T IsSmallerOrEqualThan(object? value)
        => AddFilterWithOperator(new IsSmallerOrEqualOperatorBuilder(), value);

    public T IsSmallerThan(object? value)
        => AddFilterWithOperator(new IsSmallerOperatorBuilder(), value);

    public T DoesNotContain(object? value)
        => AddFilterWithOperator(new StringNotContainsOperatorBuilder(), value);

    public T DoesNotEndWith(object? value)
        => AddFilterWithOperator(new NotEndsWithOperatorBuilder(), value);

    public T IsNotEqualTo(object? value)
        => AddFilterWithOperator(new NotEqualsOperatorBuilder(), value);

    public T DoesNotStartWith(object? value)
        => AddFilterWithOperator(new NotStartsWithOperatorBuilder(), value);

    public T StartsWith(object? value)
        => AddFilterWithOperator(new StartsWithOperatorBuilder(), value);

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
