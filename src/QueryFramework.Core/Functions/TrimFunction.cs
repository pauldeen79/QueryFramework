using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record TrimFunction : IQueryExpressionFunction
    {
        public TrimFunction() { }

        public TrimFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
