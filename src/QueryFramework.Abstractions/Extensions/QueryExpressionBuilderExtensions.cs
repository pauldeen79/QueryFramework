using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions.Builders;

namespace QueryFramework.Abstractions.Extensions
{
    public static partial class QueryExpressionBuilderExtensions
    {
        public static T WithFieldName<T>(this T instance, string fieldName)
            where T : IQueryExpressionBuilder
            => instance.Chain(x => x.FieldName = fieldName);

        public static T WithFunction<T>(this T instance, IQueryExpressionFunctionBuilder? function)
            where T : IQueryExpressionBuilder
            => instance.Chain(x => x.Function = function);
    }
}
