﻿namespace QueryFramework.Abstractions;

public class ComposableEvaluatableBuilderWrapper<T> where T : IQueryBuilder
{
    private readonly T _instance;
    private readonly string _fieldName;
    private readonly Combination? _combination;
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
}
