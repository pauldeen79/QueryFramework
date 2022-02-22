namespace QueryFramework.Core.Extensions;

public static class QueryExpressionBuilderExtensions
{
    public static IExpressionFunction? GetFunction(this IExpressionBuilder instance)
        => instance.Build() as IExpressionFunction ?? instance?.Function?.Build();

    #region Built-in functions
    /// <summary>Gets the length of this expression.</summary>
    public static IExpressionBuilder Len(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new LengthFunctionBuilder().WithInnerFunction(instance));

    /// <summary>Trims the value of this expression.</summary>
    public static IExpressionBuilder Trim(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new TrimFunctionBuilder().WithInnerFunction(instance));

    /// <summary>Gets the upper-cased value of this expression.</summary>
    public static IExpressionBuilder Upper(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new UpperFunctionBuilder().WithInnerFunction(instance));

    /// <summary>Gets the lower-cased value of this expression.</summary>
    public static IExpressionBuilder Lower(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new LowerFunctionBuilder().WithInnerFunction(instance));

    /// <summary>Gets the left part of this expression.</summary>
    public static IExpressionBuilder Left(this IExpressionBuilder instance, int length)
        => instance.Chain(x => x.Function = new LeftFunctionBuilder().WithInnerFunction(instance).WithLength(length));

    /// <summary>Gets the right part of this expression.</summary>
    public static IExpressionBuilder Right(this IExpressionBuilder instance, int length)
        => instance.Chain(x => x.Function = new RightFunctionBuilder().WithInnerFunction(instance).WithLength(length));

    /// <summary>Gets the year of this date expression.</summary>
    public static IExpressionBuilder Year(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new YearFunctionBuilder().WithInnerFunction(instance));

    /// <summary>Gets the month of this date expression.</summary>
    public static IExpressionBuilder Month(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new MonthFunctionBuilder().WithInnerFunction(instance));

    /// <summary>Gets the day of this date expression.</summary>
    public static IExpressionBuilder Day(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new DayFunctionBuilder().WithInnerFunction(instance));

    /// <summary>Gets the count of this expression.</summary>
    public static IExpressionBuilder Count(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new CountFunctionBuilder().WithInnerFunction(instance));

    /// <summary>Gets the sum of this expression.</summary>
    public static IExpressionBuilder Sum(this IExpressionBuilder instance)
        => instance.Chain(x => x.Function = new SumFunctionBuilder().WithInnerFunction(instance));
    #endregion
}
