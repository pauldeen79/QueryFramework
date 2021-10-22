using System;
using System.Collections.Generic;
using QueryFramework.Abstractions;

namespace QueryFramework.InMemory
{
    internal sealed class OrderByWrapper<T> : IComparable<OrderByWrapper<T>>, IEquatable<OrderByWrapper<T>>, IComparable
    {
        public OrderByWrapper(T wrappedItem,
                              IReadOnlyCollection<IQuerySortOrder> orderByFields,
                              IExpressionEvaluator<T> valueRetriever)
        {
            WrappedItem = wrappedItem;
            OrderByFields = orderByFields;
            ValueRetriever = valueRetriever;
        }

        public T WrappedItem { get; }
        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
        public IExpressionEvaluator<T> ValueRetriever { get; }

        public int CompareTo(OrderByWrapper<T> other)
        {
            foreach (var orderByField in OrderByFields)
            {
                var currentValue = ValueRetriever.GetValue(WrappedItem, orderByField.Field) as IComparable;
                var otherValue = ValueRetriever.GetValue(other.WrappedItem, orderByField.Field);
                if (currentValue == null && otherValue == null)
                {
                    continue;
                }
                if (currentValue == null)
                {
                    return orderByField.Order == QuerySortOrderDirection.Ascending
                        ? - 1
                        : 1;
                }
                if (otherValue == null)
                {
                    return orderByField.Order == QuerySortOrderDirection.Ascending
                        ? 1
                        : -1;
                }
                var result = currentValue.CompareTo(otherValue);
                if (result != 0)
                {
                    return orderByField.Order == QuerySortOrderDirection.Ascending
                        ? result
                        : -result;
                }
            }

            return 0;
        }

        public int CompareTo(object obj)
            => obj is OrderByWrapper<T> wrapper
                ? CompareTo(wrapper)
                : 1;

        public override bool Equals(object obj)
            => Equals(obj as OrderByWrapper<T>);

        public bool Equals(OrderByWrapper<T> other)
            => CompareTo(other) == 0;

        public override int GetHashCode()
        {
            var hashCode = -521269828;
            foreach (var orderByField in OrderByFields)
            {
                hashCode = hashCode * -1521134295 + ValueRetriever.GetValue(WrappedItem, orderByField.Field)?.GetHashCode() ?? 0;
            }
            return hashCode;
        }

        public static bool operator ==(OrderByWrapper<T> left, OrderByWrapper<T> right)
            => left.CompareTo(right) == 0;

        public static bool operator !=(OrderByWrapper<T> left, OrderByWrapper<T> right)
            => !(left == right);

        public static bool operator >(OrderByWrapper<T> left, OrderByWrapper<T> right)
            => left.CompareTo(right) > 0;

        public static bool operator <(OrderByWrapper<T> left, OrderByWrapper<T> right)
            => left.CompareTo(right) < 0;

        public static bool operator >=(OrderByWrapper<T> left, OrderByWrapper<T> right)
            => left.CompareTo(right) >= 0;

        public static bool operator <=(OrderByWrapper<T> left, OrderByWrapper<T> right)
            => left.CompareTo(right) <= 0;
    }
}
