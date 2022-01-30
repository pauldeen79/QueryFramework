using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Core.Extensions
{
    public static class QueryExpressionFunctionBuilderExtensions
    {
        public static T WithInnerFunction<T>(this T instance, IQueryExpressionBuilder currentExpression)
            where T : IQueryExpressionFunctionBuilder
            => instance.Chain(x => x.InnerFunction = currentExpression.GetFunction()?.ToBuilder());
    }
}
