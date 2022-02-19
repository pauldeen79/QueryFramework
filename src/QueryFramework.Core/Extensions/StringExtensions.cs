namespace QueryFramework.Core.Extensions;

public static class StringExtensions
{
    #region Generated Code
    /// <summary>Creates a query condition builder with the Contains query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesContain(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.Contains)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the EndsWith query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesEndWith(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.EndsWith)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the Equal query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsEqualTo(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.Equal)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the GreterOrEqualThan query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsGreaterOrEqualThan(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.GreaterOrEqual)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsGreaterThan(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.Greater)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the IsNotNull query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static IConditionBuilder IsNotNull(this string fieldName)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.IsNotNull);

    /// <summary>Creates a query condition builder with the IsNotNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static IConditionBuilder IsNotNullOrEmpty(this string fieldName)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.IsNotNullOrEmpty);

    /// <summary>Creates a query condition builder with the IsNull query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static IConditionBuilder IsNull(this string fieldName)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.IsNull);

    /// <summary>Creates a query condition builder with the IsNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    public static IConditionBuilder IsNullOrEmpty(this string fieldName)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.IsNullOrEmpty);

    /// <summary>Creates a query condition builder with the IsLowerOrEqualThan query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsSmallerOrEqualThan(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.SmallerOrEqual)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the IsLowerThan query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsSmallerThan(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.Smaller)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the NotContains query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesNotContain(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.NotContains)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the NotEndsWith query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesNotEndWith(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.NotEndsWith)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the NotEqual query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsNotEqualTo(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.NotEqual)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the NotStartsWith query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesNotStartWith(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.NotStartsWith)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));

    /// <summary>Creates a query condition builder with the StartsWith query operator, using the specified values.</summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesStartWith(this string fieldName, object? value)
        => new ConditionBuilder().WithLeftExpression(new FieldExpressionBuilder().WithFieldName(fieldName))
                                 .WithOperator(Operator.StartsWith)
                                 .WithRightExpression(new ConstantExpressionBuilder().WithValue(value));
    #endregion

    #region Built-in functions
    /// <summary>Gets the length of this field.</summary>
    public static IExpressionBuilder Len(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new LengthFunctionBuilder());

    /// <summary>Trims the value of this field.</summary>
    public static IExpressionBuilder SqlTrim(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new TrimFunctionBuilder());

    /// <summary>Gets the upper-cased value of this field.</summary>
    public static IExpressionBuilder Upper(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new UpperFunctionBuilder());

    /// <summary>Gets the lower-cased value of this field.</summary>
    public static IExpressionBuilder Lower(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new LowerFunctionBuilder());

    /// <summary>Gets the left part of this expression.</summary>
    /// <param name="length">Number of positions</param>
    public static IExpressionBuilder Left(this string fieldName, int length)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new LeftFunctionBuilder().WithLength(length));

    /// <summary>Gets the right part of this expression.</summary>
    /// <param name="length">Number of positions</param>
    public static IExpressionBuilder Right(this string fieldName, int length)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new RightFunctionBuilder().WithLength(length));

    /// <summary>Gets the year of this date field.</summary>
    public static IExpressionBuilder Year(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new YearFunctionBuilder());

    /// <summary>Gets the month of this date field.</summary>
    public static IExpressionBuilder Month(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new MonthFunctionBuilder());

    /// <summary>Gets the day of this date expression.</summary>
    public static IExpressionBuilder Day(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new DayFunctionBuilder());

    /// <summary>Gets the count of this field.</summary>
    public static IExpressionBuilder Count(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new CountFunctionBuilder());

    /// <summary>Gets the sum of this field.</summary>
    public static IExpressionBuilder Sum(this string fieldName)
        => new FieldExpressionBuilder().WithFieldName(fieldName).WithFunction(new SumFunctionBuilder());
    #endregion
}
