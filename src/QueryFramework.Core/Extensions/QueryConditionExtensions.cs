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
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder DoesContain(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.Contains,
                Value = value
            };

        /// <summary>Creates a query expression with the EndsWith query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder DoesEndWith(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.EndsWith,
                Value = value
            };

        /// <summary>Creates a query expression with the Equals query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder IsEqualTo(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.Equal,
                Value = value
            };

        /// <summary>Creates a query expression with the GreaterOrEqualThan query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder IsGreaterOrEqualThan(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.GreaterOrEqual,
                Value = value
            };

        /// <summary>Creates a query expression with the GreaterThan query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder IsGreaterThan(this QueryExpressionBuilder instance, object? value = null)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.Greater,
                Value = value
            };

        /// <summary>Creates a query expression with the IsNotNull query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        public static QueryConditionBuilder IsNotNull(this QueryExpressionBuilder instance)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.IsNotNull
            };

        /// <summary>Creates a query expression with the IsNotNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        public static QueryConditionBuilder IsNotNullOrEmpty(this QueryExpressionBuilder instance)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.IsNotNullOrEmpty
            };

        /// <summary>Creates a query expression with the IsNull query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        public static QueryConditionBuilder IsNull(this QueryExpressionBuilder instance)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.IsNull
            };

        /// <summary>Creates a query expression with the IsNullOrEmpty query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        public static QueryConditionBuilder IsNullOrEmpty(this QueryExpressionBuilder instance)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.IsNullOrEmpty
            };

        /// <summary>Creates a query expression with the LowerOrEqualThan query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder IsLowerOrEqualThan(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.LowerOrEqual,
                Value = value
            };

        /// <summary>Creates a query expression with the LowerTHan query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder IsLowerThan(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.Lower,
                Value = value
            };

        /// <summary>Creates a query expression with the NotContains query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder DoesNotContain(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.NotContains,
                Value = value
            };

        /// <summary>Creates a query expression with the NotEndsWith query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static QueryConditionBuilder DoesNotEndWith(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.NotEndsWith,
                Value = value
            };

        /// <summary>Creates a query expression with the NotEqual query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static QueryConditionBuilder IsNotEqualTo(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.NotEqual,
                Value = value
            };

        /// <summary>Creates a query expression with the NotStartsWith query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        public static QueryConditionBuilder DoesNotStartWith(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.NotStartsWith,
                Value = value
            };

        /// <summary>Creates a query expression with the StartsWith query operator, using the specified values.</summary>
        /// <param name="instance">The query expression builder instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="openBracket">if set to <c>true</c> [open bracket].</param>
        /// <param name="closeBracket">if set to <c>true</c> [close bracket].</param>
        /// <param name="combination">The combination.</param>
        public static QueryConditionBuilder DoesStartWith(this QueryExpressionBuilder instance, object? value)
            => new QueryConditionBuilder()
            {
                Field = instance,
                Operator = QueryOperator.StartsWith,
                Value = value
            };
        #endregion
    }
}
