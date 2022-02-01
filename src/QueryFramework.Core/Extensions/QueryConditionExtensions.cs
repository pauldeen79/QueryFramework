using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class QueryConditionExtensions
    {
        #region Generated code
        /// <summary>Creates a query condition builder with the Contains query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesContain(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.Contains,
                Value = value
            };

        /// <summary>Creates a query condition builder with the EndsWith query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesEndWith(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.EndsWith,
                Value = value
            };

        /// <summary>Creates a query condition builder with the Equals query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsEqualTo(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.Equal,
                Value = value
            };

        /// <summary>Creates a query condition builder with the GreaterOrEqualThan query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsGreaterOrEqualThan(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.GreaterOrEqual,
                Value = value
            };

        /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsGreaterThan(this IQueryExpressionBuilder instance, object? value = null)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.Greater,
                Value = value
            };

        /// <summary>Creates a query condition builder with the IsNotNull query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        public static IQueryConditionBuilder IsNotNull(this IQueryExpressionBuilder instance)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.IsNotNull
            };

        /// <summary>Creates a query condition builder with the IsNotNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        public static IQueryConditionBuilder IsNotNullOrEmpty(this IQueryExpressionBuilder instance)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.IsNotNullOrEmpty
            };

        /// <summary>Creates a query condition builder with the IsNull query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        public static IQueryConditionBuilder IsNull(this IQueryExpressionBuilder instance)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.IsNull
            };

        /// <summary>Creates a query condition builder with the IsNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        public static IQueryConditionBuilder IsNullOrEmpty(this IQueryExpressionBuilder instance)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.IsNullOrEmpty
            };

        /// <summary>Creates a query condition builder with the LowerOrEqualThan query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsLowerOrEqualThan(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.LowerOrEqual,
                Value = value
            };

        /// <summary>Creates a query condition builder with the LowerTHan query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder IsLowerThan(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.Lower,
                Value = value
            };

        /// <summary>Creates a query condition builder with the NotContains query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesNotContain(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.NotContains,
                Value = value
            };

        /// <summary>Creates a query condition builder with the NotEndsWith query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryConditionBuilder DoesNotEndWith(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.NotEndsWith,
                Value = value
            };

        /// <summary>Creates a query condition builder with the NotEqual query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryConditionBuilder IsNotEqualTo(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.NotEqual,
                Value = value
            };

        /// <summary>Creates a query condition builder with the NotStartsWith query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static IQueryConditionBuilder DoesNotStartWith(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.NotStartsWith,
                Value = value
            };

        /// <summary>Creates a query condition builder with the StartsWith query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static IQueryConditionBuilder DoesStartWith(this IQueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.StartsWith,
                Value = value
            };
        #endregion
    }
}
