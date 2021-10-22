using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class QueryConditionExtensions
    {
        /// <summary>Creates a new instance from the current instance, with the specified values. (or the same value, if it's not specified)</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="openBracket">The open bracket.</param>
        /// <param name="closeBracket">The close bracket.</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition With(this IQueryCondition instance,
                                           bool? openBracket = null,
                                           bool? closeBracket = null,
                                           QueryCombination? combination = null)
            => instance is ICustomQueryCondition cqc
                ? cqc.With(openBracket, closeBracket, combination)
                : new QueryCondition
                (
                    instance.Field,
                    instance.Operator,
                    instance.Value,
                    openBracket ?? instance.OpenBracket,
                    closeBracket ?? instance.CloseBracket,
                    combination ?? instance.Combination
                );

        public static IQueryConditionBuilder GetBuilder(this IQueryCondition instance)
            => instance is ICustomQueryCondition custom
                ? custom.CreateBuilder()
                : new QueryConditionBuilder(instance);

        #region Generated code
        /// <summary>Creates a query expression with the Contains query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesContain
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.Contains,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the EndsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesEndWith
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.EndsWith,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the Equals query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsEqualTo
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.Equal,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the GreaterOrEqualThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsGreaterOrEqualThan
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.GreaterOrEqual,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the GreaterThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsGreaterThan
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.Greater,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the IsNotNull query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNotNull
        (
            this IQueryExpression fieldName,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.IsNotNull,
                null,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the IsNotNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNotNullOrEmpty
        (
            this IQueryExpression fieldName,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.IsNotNullOrEmpty,
                null,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the IsNull query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNull
        (
            this IQueryExpression fieldName,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.IsNull,
                null,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the IsNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNullOrEmpty
        (
            this IQueryExpression fieldName,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.IsNullOrEmpty,
                null,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the LowerOrEqualThan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsLowerOrEqualThan
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.LowerOrEqual,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the LowerTHan query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsLowerThan
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.Lower,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the NotContains query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesNotContain
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.NotContains,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the NotEndsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesNotEndWith
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.NotEndsWith,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the NotEqual query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition IsNotEqualTo
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.NotEqual,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the NotStartsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesNotStartWith
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.NotStartsWith,
                value,
                openBracket,
                closeBracket,
                combination
            );

        /// <summary>Creates a query expression with the StartsWith query operator, using the specified values.</summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryCondition DoesStartWith
        (
            this IQueryExpression fieldName,
            object value = null,
            bool openBracket = false,
            bool closeBracket = false,
            QueryCombination combination = QueryCombination.And
        ) => new QueryCondition
            (
                fieldName,
                QueryOperator.StartsWith,
                value,
                openBracket,
                closeBracket,
                combination
            );
        #endregion
    }
}
