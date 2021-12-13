using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class QueryExpressionExtensions
    {
        /// <summary>Creates a new instance from the current instance, with the specified values. (or the same value, if it's not specified)</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="function">The function.</param>
        public static IQueryExpression With(this IQueryExpression instance,
                                            string fieldName = "",
                                            IQueryExpressionFunction? function = null)
            => new QueryExpression(fieldName == string.Empty
                                        ? instance.FieldName
                                        : fieldName,
                                   function == null
                                        ? instance.Function
                                        : function);

        public static IQueryExpressionBuilder ToBuilder(this IQueryExpression instance)
            => instance is ICustomQueryExpression customQueryExpression
                ? customQueryExpression.CreateBuilder()
                : new QueryExpressionBuilder(instance);
    }
}
