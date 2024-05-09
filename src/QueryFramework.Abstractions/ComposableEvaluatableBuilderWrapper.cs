namespace QueryFramework.Abstractions;

public class ComposableEvaluatableBuilderWrapper<T> where T : IQueryBuilder
{
    private readonly T _instance;
    private readonly string _fieldName;
    private Combination? _combination;
    private bool _startGroup;
    private bool _endGroup;
    private ExpressionBuilder? _expression;

    public ComposableEvaluatableBuilderWrapper(T instance, string fieldName, Combination? combination = null)
    {
        _instance = instance.IsNotNull(nameof(instance));
        _fieldName = fieldName.IsNotNull(nameof(_fieldName));
        _combination = combination;
    }

    #region Generated code
    public T Contains(string value)
        => AddFilterWithOperator(new StringContainsOperatorBuilder(), value);

    public T Contains(Func<string> valueDelegate)
        => AddFilterWithOperator(new StringContainsOperatorBuilder(), valueDelegate);

    public T Contains<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new StringContainsOperatorBuilder(), expression);

    public T EndsWith(string value)
        => AddFilterWithOperator(new EndsWithOperatorBuilder(), value);

    public T EndsWith(Func<string> valueDelegate)
        => AddFilterWithOperator(new EndsWithOperatorBuilder(), valueDelegate);

    public T EndsWith<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new EndsWithOperatorBuilder(), expression);

    public T IsEqualTo(object? value)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), value);

    public T IsEqualTo<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), valueDelegate);

    public T IsEqualTo(ExpressionBuilder expression)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), expression);

    public T IsGreaterOrEqualThan(object? value)
        => AddFilterWithOperator(new IsGreaterOrEqualOperatorBuilder(), value);

    public T IsGreaterOrEqualThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new IsGreaterOrEqualOperatorBuilder(), valueDelegate);

    public T IsGreaterOrEqualThan(ExpressionBuilder expression)
        => AddFilterWithOperator(new IsGreaterOrEqualOperatorBuilder(), expression);

    public T IsGreaterThan(object? value)
        => AddFilterWithOperator(new IsGreaterOperatorBuilder(), value);

    public T IsGreaterThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new IsGreaterOperatorBuilder(), valueDelegate);

    public T IsGreaterThan(ExpressionBuilder expression)
        => AddFilterWithOperator(new IsGreaterOperatorBuilder(), expression);

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

    public T IsSmallerOrEqualThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new IsSmallerOrEqualOperatorBuilder(), valueDelegate);

    public T IsSmallerOrEqualThan(ExpressionBuilder expression)
        => AddFilterWithOperator(new IsSmallerOrEqualOperatorBuilder(), expression);

    public T IsSmallerThan(object? value)
        => AddFilterWithOperator(new IsSmallerOperatorBuilder(), value);

    public T IsSmallerThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new IsSmallerOperatorBuilder(), valueDelegate);

    public T IsSmallerThan(ExpressionBuilder expression)
        => AddFilterWithOperator(new IsSmallerOperatorBuilder(), expression);

    public T DoesNotContain(string value)
        => AddFilterWithOperator(new StringNotContainsOperatorBuilder(), value);

    public T DoesNotContain(Func<string> valueDelegate)
        => AddFilterWithOperator(new StringNotContainsOperatorBuilder(), valueDelegate);

    public T DoesNotContain<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new StringNotContainsOperatorBuilder(), expression);

    public T DoesNotEndWith(string value)
        => AddFilterWithOperator(new NotEndsWithOperatorBuilder(), value);

    public T DoesNotEndWith(Func<string> valueDelegate)
        => AddFilterWithOperator(new NotEndsWithOperatorBuilder(), valueDelegate);

    public T DoesNotEndWith<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new NotEndsWithOperatorBuilder(), expression);

    public T IsNotEqualTo(object? value)
        => AddFilterWithOperator(new NotEqualsOperatorBuilder(), value);

    public T IsNotEqualTo<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new NotEqualsOperatorBuilder(), valueDelegate);

    public T IsNotEqualTo(ExpressionBuilder expression)
        => AddFilterWithOperator(new NotEqualsOperatorBuilder(), expression);

    public T DoesNotStartWith(string value)
        => AddFilterWithOperator(new NotStartsWithOperatorBuilder(), value);

    public T DoesNotStartWith(Func<string> valueDelegate)
        => AddFilterWithOperator(new NotStartsWithOperatorBuilder(), valueDelegate);

    public T DoesNotStartWith<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new NotStartsWithOperatorBuilder(), expression);

    public T StartsWith(string value)
        => AddFilterWithOperator(new StartsWithOperatorBuilder(), value);

    public T StartsWith(Func<string> valueDelegate)
        => AddFilterWithOperator(new StartsWithOperatorBuilder(), valueDelegate);

    public T StartsWith<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new StartsWithOperatorBuilder(), expression);
    #endregion

    #region Built-in functions
    /// <summary>Gets the length of this field.</summary>
    public ComposableEvaluatableBuilderWrapper<T> Len()
    {
        _expression = new StringLengthExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Trims the value of this field.</summary>
    public ComposableEvaluatableBuilderWrapper<T> SqlTrim()
    {
        _expression = new TrimExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the upper-cased value of this field.</summary>
    public ComposableEvaluatableBuilderWrapper<T> Upper()
    {
        _expression = new ToUpperCaseExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the lower-cased value of this field.</summary>
    public ComposableEvaluatableBuilderWrapper<T> Lower()
    {
        _expression = new ToLowerCaseExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the left part of this expression.</summary>
    /// <param name="length">Number of positions</param>
    public ComposableEvaluatableBuilderWrapper<T> Left(int length)
    {
        _expression = new LeftExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName)).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));
        return this;
    }

    /// <summary>Gets the right part of this expression.</summary>
    /// <param name="length">Number of positions</param>
    public ComposableEvaluatableBuilderWrapper<T> Right(int length)
    {
        _expression = new RightExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName)).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));
        return this;
    }

    /// <summary>Gets the year of this date field.</summary>
    public ComposableEvaluatableBuilderWrapper<T> Year()
    {
        _expression = new YearExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the month of this date field.</summary>
    public ComposableEvaluatableBuilderWrapper<T> Month()
    {
        _expression = new MonthExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the day of this date expression.</summary>
    public ComposableEvaluatableBuilderWrapper<T> Day()
    {
        _expression = new DayExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the count of this field.</summary>
    public ComposableEvaluatableBuilderWrapper<T> Count()
    {
        _expression = new CountExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<IEnumerable>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }

    /// <summary>Gets the sum of this field.</summary>
    public ComposableEvaluatableBuilderWrapper<T> Sum()
    {
        _expression = new SumExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<IEnumerable>().WithExpression(new ContextExpressionBuilder()).WithFieldName(_fieldName));
        return this;
    }
    #endregion

    private T AddFilterWithOperator(OperatorBuilder @operator, object? value)
        => _instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, value, _combination, _startGroup, _endGroup, _expression));

    private T AddFilterWithOperator<TValue>(OperatorBuilder @operator, Func<TValue> valueDelegate)
        => _instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, valueDelegate, _combination, _startGroup, _endGroup, _expression));

    private T AddFilterWithOperator(OperatorBuilder @operator, ExpressionBuilder expression)
        => _instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, expression, _combination, _startGroup, _endGroup, _expression));

    private T AddFilterWithOperator(OperatorBuilder @operator)
        => _instance.Where(ComposableEvaluatableBuilderHelper.Create(_fieldName, @operator, _combination, _startGroup, _endGroup, _expression));

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

    public ComposableEvaluatableBuilderWrapper<T> WithCombination(Combination combination)
    {
        _combination = combination;
        return this;
    }
}
