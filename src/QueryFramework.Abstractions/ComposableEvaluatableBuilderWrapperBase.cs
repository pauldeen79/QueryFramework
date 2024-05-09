namespace QueryFramework.Abstractions;

public abstract class ComposableEvaluatableBuilderWrapperBase<T>
    where T : IQueryBuilder
{
    private readonly ExpressionBuilder _leftExpression;

    protected T Instance { get; }

    protected Combination? Combination { get; set; }
    protected bool StartGroup { get; set; }
    protected bool EndGroup { get; set; }

    protected ComposableEvaluatableBuilderWrapperBase(T instance, ExpressionBuilder leftExpression, Combination? combination = null)
    {
        Instance = instance.IsNotNull(nameof(instance));
        _leftExpression = leftExpression.IsNotNull(nameof(leftExpression));
        Combination = combination;
    }

    #region Generated code
    public T Contains(string value)
        => AddFilterWithOperator(new StringContainsOperatorBuilder(), value);

    public T Contains(Func<string> valueDelegate)
        => AddFilterWithOperator(new StringContainsOperatorBuilder(), valueDelegate);

    public T Contains<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new StringContainsOperatorBuilder(), (ExpressionBuilder)expression);

    public T EndsWith(string value)
        => AddFilterWithOperator(new EndsWithOperatorBuilder(), value);

    public T EndsWith(Func<string> valueDelegate)
        => AddFilterWithOperator(new EndsWithOperatorBuilder(), valueDelegate);

    public T EndsWith<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new EndsWithOperatorBuilder(), (ExpressionBuilder)expression);

    public T IsEqualTo<TValue>(TValue value)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), value);

    public T IsEqualTo<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), valueDelegate);

    public T IsEqualToParameter(string parameterName)
        => AddFilterWithOperator(new EqualsOperatorBuilder(), new QueryParameterExpressionBuilder().WithParameterName(parameterName));

    public T IsGreaterOrEqualThan<TValue>(TValue value)
        => AddFilterWithOperator(new IsGreaterOrEqualOperatorBuilder(), value);

    public T IsGreaterOrEqualThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new IsGreaterOrEqualOperatorBuilder(), valueDelegate);

    public T IsGreaterThan<TValue>(TValue value)
        => AddFilterWithOperator(new IsGreaterOperatorBuilder(), value);

    public T IsGreaterThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new IsGreaterOperatorBuilder(), valueDelegate);

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

    public T IsSmallerOrEqualThan<TValue>(TValue value)
        => AddFilterWithOperator(new IsSmallerOrEqualOperatorBuilder(), value);

    public T IsSmallerOrEqualThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new IsSmallerOrEqualOperatorBuilder(), valueDelegate);

    public T IsSmallerThan<TValue>(TValue value)
        => AddFilterWithOperator(new IsSmallerOperatorBuilder(), value);

    public T IsSmallerThan<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new IsSmallerOperatorBuilder(), valueDelegate);

    public T DoesNotContain(string value)
        => AddFilterWithOperator(new StringNotContainsOperatorBuilder(), value);

    public T DoesNotContain(Func<string> valueDelegate)
        => AddFilterWithOperator(new StringNotContainsOperatorBuilder(), valueDelegate);

    public T DoesNotContain<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new StringNotContainsOperatorBuilder(), (ExpressionBuilder)expression);

    public T DoesNotEndWith(string value)
        => AddFilterWithOperator(new NotEndsWithOperatorBuilder(), value);

    public T DoesNotEndWith(Func<string> valueDelegate)
        => AddFilterWithOperator(new NotEndsWithOperatorBuilder(), valueDelegate);

    public T DoesNotEndWith<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new NotEndsWithOperatorBuilder(), (ExpressionBuilder)expression);

    public T IsNotEqualTo<TValue>(TValue value)
        => AddFilterWithOperator(new NotEqualsOperatorBuilder(), value);

    public T IsNotEqualTo<TValue>(Func<TValue> valueDelegate)
        => AddFilterWithOperator(new NotEqualsOperatorBuilder(), valueDelegate);

    public T DoesNotStartWith(string value)
        => AddFilterWithOperator(new NotStartsWithOperatorBuilder(), value);

    public T DoesNotStartWith(Func<string> valueDelegate)
        => AddFilterWithOperator(new NotStartsWithOperatorBuilder(), valueDelegate);

    public T DoesNotStartWith<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new NotStartsWithOperatorBuilder(), (ExpressionBuilder)expression);

    public T StartsWith(string value)
        => AddFilterWithOperator(new StartsWithOperatorBuilder(), value);

    public T StartsWith(Func<string> valueDelegate)
        => AddFilterWithOperator(new StartsWithOperatorBuilder(), valueDelegate);

    public T StartsWith<TExpression>(TExpression expression)
        where TExpression : ExpressionBuilder, ITypedExpressionBuilder<string>
        => AddFilterWithOperator(new StartsWithOperatorBuilder(), (ExpressionBuilder)expression);
    #endregion

    protected virtual T AddFilterWithOperator<TValue>(OperatorBuilder @operator, TValue value)
        => value is ExpressionBuilder expressionBuilder
            ? AddFilterWithOperator(@operator, expressionBuilder)
            : Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, value, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator<TValue>(OperatorBuilder @operator, Func<TValue> valueDelegate)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, valueDelegate, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator(OperatorBuilder @operator, ExpressionBuilder expression)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, expression, Combination, StartGroup, EndGroup));

    protected virtual T AddFilterWithOperator(OperatorBuilder @operator)
        => Instance.Where(ComposableEvaluatableBuilderHelper.Create(_leftExpression, @operator, Combination, StartGroup, EndGroup));
}
