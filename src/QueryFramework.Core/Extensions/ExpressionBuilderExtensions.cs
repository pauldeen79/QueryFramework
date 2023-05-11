namespace QueryFramework.Core.Extensions;

public static class ExpressionBuilderExtensions
{
    public static ITypedExpressionBuilder<T> Cast<T>(this ExpressionBuilder builder)
        => new CastExpressionBuilder<T>().WithSourceExpression(builder);

    public static ITypedExpressionBuilder<TTarget> Cast<TSource, TTarget>(this ITypedExpressionBuilder<TSource> builder)
        => new CastExpressionBuilder<TTarget>()
            .WithSourceExpression(builder as ExpressionBuilder
                ?? new InvalidExpressionBuilder().WithErrorMessageExpression("Could not convert typed expression builder to expression builder"));

    #region Generated code
    /// <summary>Creates a query condition builder with the Contains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesContain(this ExpressionBuilder instance, string value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new StringContainsOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the EndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesEndWith(this ExpressionBuilder instance, string value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new EndsWithOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the Equals query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsEqualTo(this ExpressionBuilder instance, object? value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new EqualsOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the GreaterOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsGreaterOrEqualThan(this ExpressionBuilder instance, object? value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsGreaterOrEqualOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsGreaterThan(this ExpressionBuilder instance, object? value = null)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsGreaterOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the IsNotNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static ComposableEvaluatableBuilder IsNotNull(this ExpressionBuilder instance)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNotNullOperatorBuilder(),
            RightExpression = new EmptyExpressionBuilder(),
        };

    /// <summary>Creates a query condition builder with the IsNotNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static ComposableEvaluatableBuilder IsNotNullOrEmpty(this ExpressionBuilder instance)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNotNullOrEmptyOperatorBuilder(),
            RightExpression = new EmptyExpressionBuilder(),
        };

    /// <summary>Creates a query condition builder with the IsNotNullOrWhiteSpace query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static ComposableEvaluatableBuilder IsNotNullOrWhiteSpace(this ExpressionBuilder instance)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNotNullOrWhiteSpaceOperatorBuilder(),
            RightExpression = new EmptyExpressionBuilder(),
        };

    /// <summary>Creates a query condition builder with the IsNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static ComposableEvaluatableBuilder IsNull(this ExpressionBuilder instance)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNullOperatorBuilder(),
            RightExpression = new EmptyExpressionBuilder(),
        };

    /// <summary>Creates a query condition builder with the IsNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static ComposableEvaluatableBuilder IsNullOrEmpty(this ExpressionBuilder instance)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNullOrEmptyOperatorBuilder(),
            RightExpression = new EmptyExpressionBuilder(),
        };

    /// <summary>Creates a query condition builder with the IsNullOrWhiteSpace query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static ComposableEvaluatableBuilder IsNullOrWhiteSpace(this ExpressionBuilder instance)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNullOrWhiteSpaceOperatorBuilder(),
            RightExpression = new EmptyExpressionBuilder(),
        };

    /// <summary>Creates a query condition builder with the LowerOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsSmallerOrEqualThan(this ExpressionBuilder instance, object? value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsSmallerOrEqualOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the LowerTHan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsSmallerThan(this ExpressionBuilder instance, object? value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsSmallerOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotContains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesNotContain(this ExpressionBuilder instance, string value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new StringNotContainsOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotEndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
    /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
    /// <param name="combination">The combination.</param>
    public static ComposableEvaluatableBuilder DoesNotEndWith(this ExpressionBuilder instance, string value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new NotEndsWithOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotEqual query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
    /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
    /// <param name="combination">The combination.</param>
    public static ComposableEvaluatableBuilder IsNotEqualTo(this ExpressionBuilder instance, object? value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new NotEqualsOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotStartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesNotStartWith(this ExpressionBuilder instance, string value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new NotStartsWithOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the StartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
    /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
    /// <param name="combination">The combination.</param>
    public static ComposableEvaluatableBuilder DoesStartWith(this ExpressionBuilder instance, string value)
        => new ComposableEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new StartsWithOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };
    #endregion

    #region Built-in functions
    /// <summary>Gets the length of this expression.</summary>
    public static ITypedExpressionBuilder<int> Len(this ITypedExpressionBuilder<string> instance)
        => new StringLengthExpressionBuilder().WithExpression(instance);

    /// <summary>Trims the value of this expression.</summary>
    public static ITypedExpressionBuilder<string> Trim(this ITypedExpressionBuilder<string> instance)
        => new TrimExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the upper-cased value of this expression.</summary>
    public static ITypedExpressionBuilder<string> Upper(this ITypedExpressionBuilder<string> instance)
        => new ToUpperCaseExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the lower-cased value of this expression.</summary>
    public static ITypedExpressionBuilder<string> Lower(this ITypedExpressionBuilder<string> instance)
        => new ToLowerCaseExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the left part of this expression.</summary>
    public static ITypedExpressionBuilder<string> Left(this ITypedExpressionBuilder<string> instance, int length)
        => new LeftExpressionBuilder().WithExpression(instance).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));

    /// <summary>Gets the right part of this expression.</summary>
    public static ITypedExpressionBuilder<string> Right(this ITypedExpressionBuilder<string> instance, int length)
        => new RightExpressionBuilder().WithExpression(instance).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));

    /// <summary>Gets the year of this date expression.</summary>
    public static ITypedExpressionBuilder<int> Year(this ITypedExpressionBuilder<DateTime> instance)
        => new YearExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the month of this date expression.</summary>
    public static ITypedExpressionBuilder<int> Month(this ITypedExpressionBuilder<DateTime> instance)
        => new MonthExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the day of this date expression.</summary>
    public static ITypedExpressionBuilder<int> Day(this ITypedExpressionBuilder<DateTime> instance)
        => new DayExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the count of this expression.</summary>
    public static ITypedExpressionBuilder<int> Count(this ITypedExpressionBuilder<IEnumerable> instance)
        => new CountExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the sum of this expression.</summary>
    public static ExpressionBuilder Sum(this ITypedExpressionBuilder<IEnumerable> instance)
        => new SumExpressionBuilder().WithExpression(instance);
    #endregion
}
