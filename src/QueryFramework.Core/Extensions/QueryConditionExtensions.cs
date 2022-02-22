namespace QueryFramework.Core.Extensions;

public static class QueryConditionExtensions
{
    #region Generated code
    /// <summary>Creates a query condition builder with the Contains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesContain(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.Contains,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the EndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesEndWith(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.EndsWith,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the Equals query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsEqualTo(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.Equal,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the GreaterOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsGreaterOrEqualThan(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.GreaterOrEqual,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsGreaterThan(this IExpressionBuilder instance, object? value = null)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.Greater,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the IsNotNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNotNull(this IExpressionBuilder instance)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.IsNotNull
        };

    /// <summary>Creates a query condition builder with the IsNotNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNotNullOrEmpty(this IExpressionBuilder instance)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.IsNotNullOrEmpty
        };

    /// <summary>Creates a query condition builder with the IsNotNullOrWhiteSpace query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNotNullOrWhiteSpace(this IExpressionBuilder instance)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.IsNotNullOrWhiteSpace
        };

    /// <summary>Creates a query condition builder with the IsNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNull(this IExpressionBuilder instance)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.IsNull
        };

    /// <summary>Creates a query condition builder with the IsNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNullOrEmpty(this IExpressionBuilder instance)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.IsNullOrEmpty
        };

    /// <summary>Creates a query condition builder with the IsNullOrWhiteSpace query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNullOrWhiteSpace(this IExpressionBuilder instance)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.IsNullOrWhiteSpace
        };

    /// <summary>Creates a query condition builder with the LowerOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsSmallerOrEqualThan(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.SmallerOrEqual,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the LowerTHan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsSmallerThan(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.Smaller,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotContains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesNotContain(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.NotContains,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotEndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
    /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
    /// <param name="combination">The combination.</param>
    public static IConditionBuilder DoesNotEndWith(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.NotEndsWith,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotEqual query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
    /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
    /// <param name="combination">The combination.</param>
    public static IConditionBuilder IsNotEqualTo(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.NotEqual,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the NotStartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesNotStartWith(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.NotStartsWith,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };

    /// <summary>Creates a query condition builder with the StartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
    /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
    /// <param name="combination">The combination.</param>
    public static IConditionBuilder DoesStartWith(this IExpressionBuilder instance, object? value)
        => new ConditionBuilder()
        {
            LeftExpression = instance,
            Operator = Operator.StartsWith,
            RightExpression = new ConstantExpressionBuilder().WithValue(value)
        };
    #endregion
}
