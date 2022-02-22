namespace QueryFramework.SqlServer.Extensions;

public static class CombinationExtensions
{
    public static string ToSql(this Combination instance)
        => instance switch
        {
            Combination.And => "AND",
            Combination.Or => "OR",
            _ => throw new ArgumentOutOfRangeException(nameof(instance), $"Unsupported combination: {instance}")
        };
}
