namespace QueryFramework.SqlServer.Extensions;

public static class OperatorExtensions
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
            EqualsOperator => "=",
            IsGreaterOrEqualOperator => ">=",
            IsGreaterOperator => ">",
            IsSmallerOrEqualOperator => "<=",
            IsSmallerOperator => "<",
            NotEqualsOperator => "<>",
            _ => throw new ArgumentOutOfRangeException(nameof(instance), $"Unsupported query operator: {instance}"),
        };

    /// <summary>
    /// Creates a NOT keyword when the operator is negative.
    /// </summary>
    /// <param name="instance">The instance.</param>
    public static string ToNot(this Operator instance)
        => instance.GetType().In(typeof(StringNotContainsOperator),
                                 typeof(NotEndsWithOperator),
                                 typeof(NotStartsWithOperator))
            ? "NOT "
            : string.Empty;
}
