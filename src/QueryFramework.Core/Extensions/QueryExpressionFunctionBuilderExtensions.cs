namespace QueryFramework.Core.Extensions;

public static class QueryExpressionFunctionBuilderExtensions
{
    public static T WithInnerFunction<T>(this T instance, IExpressionBuilder currentExpression)
        where T : IExpressionFunctionBuilder
        => instance.Chain(x => x.InnerFunction = currentExpression.GetFunction()?.ToBuilder());
}
