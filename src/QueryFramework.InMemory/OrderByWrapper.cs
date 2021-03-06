namespace QueryFramework.InMemory;

internal sealed class OrderByWrapper : IComparable<OrderByWrapper>, IEquatable<OrderByWrapper>, IComparable
{
    public OrderByWrapper(object wrappedItem,
                          IReadOnlyCollection<IQuerySortOrder> orderByFields,
                          IExpressionEvaluator expressionEvaluator)
    {
        WrappedItem = wrappedItem;
        OrderByFields = orderByFields;
        ExpressionEvaluator = expressionEvaluator;
    }

    public object WrappedItem { get; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
    public IExpressionEvaluator ExpressionEvaluator { get; }

    public int CompareTo(OrderByWrapper other)
    {
        foreach (var orderByField in OrderByFields)
        {
            var currentValue = ExpressionEvaluator.Evaluate(WrappedItem, orderByField.Field) as IComparable;
            var otherValue = ExpressionEvaluator.Evaluate(other.WrappedItem, orderByField.Field);
            if (currentValue == null && otherValue == null)
            {
                continue;
            }
            if (currentValue == null)
            {
                return CompareToCurrentNull(orderByField);
            }
            if (otherValue == null)
            {
                return CompareToOtherNull(orderByField);
            }
            var result = currentValue.CompareTo(otherValue);
            if (result != 0)
            {
                return CompareToNotNull(orderByField, result);
            }
        }

        return 0;
    }

    public int CompareTo(object obj)
        => obj is OrderByWrapper wrapper
            ? CompareTo(wrapper)
            : 1;

    public override bool Equals(object obj)
        => obj is OrderByWrapper wrapper && Equals(wrapper);

    public bool Equals(OrderByWrapper other)
        => CompareTo(other) == 0;

    public override int GetHashCode()
    {
        var hashCode = -521269828;
        foreach (var orderByField in OrderByFields)
        {
            hashCode = hashCode * -1521134295 + ExpressionEvaluator.Evaluate(WrappedItem, orderByField.Field)?.GetHashCode() ?? 0;
        }
        return hashCode;
    }

    public static bool operator ==(OrderByWrapper left, OrderByWrapper right)
        => left.CompareTo(right) == 0;

    public static bool operator !=(OrderByWrapper left, OrderByWrapper right)
        => !(left == right);

    public static bool operator >(OrderByWrapper left, OrderByWrapper right)
        => left.CompareTo(right) > 0;

    public static bool operator <(OrderByWrapper left, OrderByWrapper right)
        => left.CompareTo(right) < 0;

    public static bool operator >=(OrderByWrapper left, OrderByWrapper right)
        => left.CompareTo(right) >= 0;

    public static bool operator <=(OrderByWrapper left, OrderByWrapper right)
        => left.CompareTo(right) <= 0;

    private static int CompareToNotNull(IQuerySortOrder orderByField, int result)
        => orderByField.Order == QuerySortOrderDirection.Ascending
            ? result
            : -result;

    private static int CompareToOtherNull(IQuerySortOrder orderByField)
        => orderByField.Order == QuerySortOrderDirection.Ascending
            ? 1
            : -1;

    private static int CompareToCurrentNull(IQuerySortOrder orderByField)
        => orderByField.Order == QuerySortOrderDirection.Ascending
            ? -1
            : 1;
}
