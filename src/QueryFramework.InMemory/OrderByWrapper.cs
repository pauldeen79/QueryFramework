﻿namespace QueryFramework.InMemory;

internal sealed class OrderByWrapper : IComparable<OrderByWrapper>, IEquatable<OrderByWrapper>, IComparable
{
    public OrderByWrapper(object wrappedItem, IReadOnlyCollection<IQuerySortOrder> orderByFields)
    {
        WrappedItem = wrappedItem;
        OrderByFields = orderByFields;
    }

    public object WrappedItem { get; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }

    public int CompareTo(OrderByWrapper other)
    {
        foreach (var orderByField in OrderByFields)
        {
            var currentValue = orderByField.FieldNameExpression.Evaluate(WrappedItem).Value as IComparable;
            var otherValue = orderByField.FieldNameExpression.Evaluate(other.WrappedItem).Value;
            if (currentValue is null && otherValue is null)
            {
                continue;
            }
            if (currentValue is null)
            {
                return CompareToCurrentNull(orderByField);
            }
            if (otherValue is null)
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
            hashCode = hashCode * -1521134295 + orderByField.FieldNameExpression.GetHashCode();
            hashCode = hashCode * -1521134295 + orderByField.Order.GetHashCode();
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
