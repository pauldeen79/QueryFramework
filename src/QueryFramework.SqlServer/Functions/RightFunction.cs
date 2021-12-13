using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record RightFunction : IQueryExpressionFunction
    {
        public RightFunction(int length) => Length = length;

        public RightFunction(int length, IQueryExpressionFunction? innerFunction)
        {
            Length = length;
            InnerFunction = innerFunction;
        }

        public string Expression => InnerFunction.GetExpression($"RIGHT({{0}}, {Length})");

        public int Length { get; }
        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
