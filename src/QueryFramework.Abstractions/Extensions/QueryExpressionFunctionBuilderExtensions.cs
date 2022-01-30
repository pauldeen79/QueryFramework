using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions
{
    public static partial class QueryExpressionFunctionBuilderExtensions
    {
        public static T WithInnerFunction<T>(this T instance, IQueryExpressionFunctionBuilder? queryExpressionFunctionBuilder)
            where T : IQueryExpressionFunctionBuilder
            => instance.Chain(x => x.InnerFunction = queryExpressionFunctionBuilder);
    }
}
