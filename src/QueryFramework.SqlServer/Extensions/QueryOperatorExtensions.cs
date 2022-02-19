namespace QueryFramework.SqlServer.Extensions;

public static class QueryOperatorExtensions
{
    /// <summary>
    /// Converts the QueryOperator to SQL.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <exception cref="ArgumentOutOfRangeException">instance</exception>
    /// <remarks>
    /// Note that only a sub set is supported. For some constructs, you have to use LIKE/LEFT/RIGHT/CHARINDEX and implement this yourself.
    /// </remarks>
    public static string ToSql(this Operator instance)
        => instance switch
        {
            Operator.Equal => "=",
            Operator.GreaterOrEqual => ">=",
            Operator.Greater => ">",
            Operator.SmallerOrEqual => "<=",
            Operator.Smaller => "<",
            Operator.NotEqual => "<>",
            _ => throw new ArgumentOutOfRangeException(nameof(instance), $"Unsupported query operator: {instance}"),
        };

    /// <summary>
    /// Creates a NOT keyword when the operator is negative.
    /// </summary>
    /// <param name="instance">The instance.</param>
    public static string ToNot(this Operator instance)
        => instance.In(Operator.NotContains,
                       Operator.NotEndsWith,
                       Operator.NotStartsWith)
            ? "NOT "
            : string.Empty;
}
