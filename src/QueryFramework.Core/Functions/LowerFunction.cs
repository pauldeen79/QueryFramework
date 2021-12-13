using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record LowerFunction : IQueryExpressionFunction
    {
        public LowerFunction() { }

        public LowerFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
