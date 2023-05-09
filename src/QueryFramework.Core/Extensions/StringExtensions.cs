namespace QueryFramework.Core.Extensions;

public static class StringExtensions
{
    #region Generated Code
    /// <summary>Creates a query condition builder with the Contains operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesContain(this string fieldName, string value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new StringContainsOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the EndsWith operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesEndWith(this string fieldName, string value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new EndsWithOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the Equal operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsEqualTo(this string fieldName, object? value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new EqualsOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the GreterOrEqualThan operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsGreaterOrEqualThan(this string fieldName, object? value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsGreaterOrEqualOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the GreaterThan operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsGreaterThan(this string fieldName, object? value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsGreaterOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the IsNotNull operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static ComposableEvaluatableBuilder IsNotNull(this string fieldName)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsNotNullOperatorBuilder())
                                             .WithRightExpression(new EmptyExpressionBuilder());

    /// <summary>Creates a query condition builder with the IsNotNullOrEmpty operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static ComposableEvaluatableBuilder IsNotNullOrEmpty(this string fieldName)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new  IsNotNullOrEmptyOperatorBuilder())
                                             .WithRightExpression(new EmptyExpressionBuilder());

    /// <summary>Creates a query condition builder with the IsNotNullOrWhiteSpace operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static ComposableEvaluatableBuilder IsNotNullOrWhiteSpace(this string fieldName)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsNotNullOrWhiteSpaceOperatorBuilder())
                                             .WithRightExpression(new EmptyExpressionBuilder());

    /// <summary>Creates a query condition builder with the IsNull operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static ComposableEvaluatableBuilder IsNull(this string fieldName)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsNullOperatorBuilder())
                                             .WithRightExpression(new EmptyExpressionBuilder());

    /// <summary>Creates a query condition builder with the IsNullOrEmpty operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static ComposableEvaluatableBuilder IsNullOrEmpty(this string fieldName)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsNullOrEmptyOperatorBuilder())
                                             .WithRightExpression(new EmptyExpressionBuilder());

    /// <summary>Creates a query condition builder with the IsNullOrWhiteSpace operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static ComposableEvaluatableBuilder IsNullOrWhiteSpace(this string fieldName)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsNullOrWhiteSpaceOperatorBuilder())
                                             .WithRightExpression(new EmptyExpressionBuilder());

    /// <summary>Creates a query condition builder with the IsLowerOrEqualThan operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsSmallerOrEqualThan(this string fieldName, object? value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsSmallerOrEqualOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the IsLowerThan operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsSmallerThan(this string fieldName, object? value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new IsSmallerOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the NotContains operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesNotContain(this string fieldName, string value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new StringNotContainsOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the NotEndsWith operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesNotEndWith(this string fieldName, string value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new NotEndsWithOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the NotEqual operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder IsNotEqualTo(this string fieldName, object? value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new NotEqualsOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the NotStartsWith operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesNotStartWith(this string fieldName, string value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new NotStartsWithOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the StartsWith operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static ComposableEvaluatableBuilder DoesStartWith(this string fieldName, string value)
        => new ComposableEvaluatableBuilder().WithLeftExpression(new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldName(fieldName))
                                             .WithOperator(new StartsWithOperatorBuilder())
                                             .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));
    #endregion

    #region Built-in functions
    /// <summary>Gets the length of this field.</summary>
    public static ITypedExpressionBuilder<int> Len(this string fieldName)
        => new StringLengthExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));

    /// <summary>Trims the value of this field.</summary>
    public static ITypedExpressionBuilder<string> SqlTrim(this string fieldName)
        => new TrimExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));

    /// <summary>Gets the upper-cased value of this field.</summary>
    public static ITypedExpressionBuilder<string> Upper(this string fieldName)
        => new ToUpperCaseExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));

    /// <summary>Gets the lower-cased value of this field.</summary>
    public static ITypedExpressionBuilder<string> Lower(this string fieldName)
        => new ToLowerCaseExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));

    /// <summary>Gets the left part of this expression.</summary>
    /// <param name="length">Number of positions</param>
    public static ITypedExpressionBuilder<string> Left(this string fieldName, int length)
        => new LeftExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName)).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));

    /// <summary>Gets the right part of this expression.</summary>
    /// <param name="length">Number of positions</param>
    public static ITypedExpressionBuilder<string> Right(this string fieldName, int length)
        => new RightExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<string>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName)).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));

    /// <summary>Gets the year of this date field.</summary>
    public static ITypedExpressionBuilder<int> Year(this string fieldName)
        => new YearExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));

    /// <summary>Gets the month of this date field.</summary>
    public static ITypedExpressionBuilder<int> Month(this string fieldName)
        => new MonthExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));

    /// <summary>Gets the day of this date expression.</summary>
    public static ITypedExpressionBuilder<int> Day(this string fieldName)
        => new DayExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<DateTime>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));

    /// <summary>Gets the count of this field.</summary>
    public static ITypedExpressionBuilder<int> Count(this string fieldName)
        => new CountExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<IEnumerable>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));

    /// <summary>Gets the sum of this field.</summary>
    public static ExpressionBuilder Sum(this string fieldName)
        => new SumExpressionBuilder().WithExpression(new TypedFieldExpressionBuilder<IEnumerable>().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(fieldName));
    #endregion
}
