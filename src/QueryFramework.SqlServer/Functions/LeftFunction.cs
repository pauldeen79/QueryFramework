using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record LeftFunction : IQueryExpressionFunction
    {
        public LeftFunction(int length) => Length = length;

        public LeftFunction(int length, IQueryExpressionFunction? innerFunction)
        {
            Length = length;
            InnerFunction = innerFunction;
        }

        public string Expression => InnerFunction.GetExpression($"LEFT({{0}}, {Length})");

        public int Length { get; }
        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
