namespace QueryFramework.Core.Extensions;

public static class ExpressionBuilderExtensions
{
    #region Generated code
    /// <summary>Creates a query condition builder with the Contains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder DoesContain(this ExpressionBuilder instance, string value)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new StringContainsOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the EndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder DoesEndWith(this ExpressionBuilder instance, string value)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new EndsWithOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the Equals query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder IsEqualTo(this ExpressionBuilder instance, object? value)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new EqualsOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the GreaterOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder IsGreaterOrEqualThan(this ExpressionBuilder instance, object? value)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsGreaterOrEqualOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder IsGreaterThan(this ExpressionBuilder instance, object? value = null)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsGreaterOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the IsNotNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static EvaluatableBuilder IsNotNull(this ExpressionBuilder instance)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNotNullOperatorBuilder()
        };

    /// <summary>Creates a query condition builder with the IsNotNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static EvaluatableBuilder IsNotNullOrEmpty(this ExpressionBuilder instance)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNotNullOrEmptyOperatorBuilder()
        };

    /// <summary>Creates a query condition builder with the IsNotNullOrWhiteSpace query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static EvaluatableBuilder IsNotNullOrWhiteSpace(this ExpressionBuilder instance)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNotNullOrWhiteSpaceOperatorBuilder()
        };

    /// <summary>Creates a query condition builder with the IsNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static EvaluatableBuilder IsNull(this ExpressionBuilder instance)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNullOperatorBuilder()
        };

    /// <summary>Creates a query condition builder with the IsNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static EvaluatableBuilder IsNullOrEmpty(this ExpressionBuilder instance)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNullOrEmptyOperatorBuilder()
        };

    /// <summary>Creates a query condition builder with the IsNullOrWhiteSpace query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static EvaluatableBuilder IsNullOrWhiteSpace(this ExpressionBuilder instance)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsNullOrWhiteSpaceOperatorBuilder()
        };

    /// <summary>Creates a query condition builder with the LowerOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder IsSmallerOrEqualThan(this ExpressionBuilder instance, object? value)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsSmallerOrEqualOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the LowerTHan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder IsSmallerThan(this ExpressionBuilder instance, object? value)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new IsSmallerOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotContains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder DoesNotContain(this ExpressionBuilder instance, string value)
        => new SingleEvaluatableBuilder()
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
    public static EvaluatableBuilder DoesNotEndWith(this ExpressionBuilder instance, string value)
        => new SingleEvaluatableBuilder()
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
    public static EvaluatableBuilder IsNotEqualTo(this ExpressionBuilder instance, object? value)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new NotEqualsOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotStartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static EvaluatableBuilder DoesNotStartWith(this ExpressionBuilder instance, string value)
        => new SingleEvaluatableBuilder()
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
    public static EvaluatableBuilder DoesStartWith(this ExpressionBuilder instance, string value)
        => new SingleEvaluatableBuilder()
        {
            LeftExpression = instance,
            Operator = new StartsWithOperatorBuilder(),
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };
    #endregion

    #region Built-in functions
    /// <summary>Gets the length of this expression.</summary>
    public static ExpressionBuilder Len(this ExpressionBuilder instance)
        => new StringLengthExpressionBuilder().WithExpression(instance);

    /// <summary>Trims the value of this expression.</summary>
    public static ExpressionBuilder Trim(this ExpressionBuilder instance)
        => new TrimExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the upper-cased value of this expression.</summary>
    public static ExpressionBuilder Upper(this ExpressionBuilder instance)
        => new ToUpperCaseExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the lower-cased value of this expression.</summary>
    public static ExpressionBuilder Lower(this ExpressionBuilder instance)
        => new ToLowerCaseExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the left part of this expression.</summary>
    public static ExpressionBuilder Left(this ExpressionBuilder instance, int length)
        => new LeftExpressionBuilder().WithExpression(instance).WithLengthExpression(new ConstantExpressionBuilder().WithValue(length));

    /// <summary>Gets the right part of this expression.</summary>
    public static ExpressionBuilder Right(this ExpressionBuilder instance, int length)
        => new RightExpressionBuilder().WithExpression(instance).WithLengthExpression(new ConstantExpressionBuilder().WithValue(length));

    /// <summary>Gets the year of this date expression.</summary>
    public static ExpressionBuilder Year(this ExpressionBuilder instance)
        => new YearExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the month of this date expression.</summary>
    public static ExpressionBuilder Month(this ExpressionBuilder instance)
        => new MonthExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the day of this date expression.</summary>
    public static ExpressionBuilder Day(this ExpressionBuilder instance)
        => new DayExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the count of this expression.</summary>
    public static ExpressionBuilder Count(this ExpressionBuilder instance)
        => new CountExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the sum of this expression.</summary>
    public static ExpressionBuilder Sum(this ExpressionBuilder instance)
        => new SumExpressionBuilder().WithExpression(instance);
    #endregion
}
