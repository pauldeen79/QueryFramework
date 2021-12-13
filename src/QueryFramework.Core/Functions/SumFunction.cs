using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record SumFunction : IQueryExpressionFunction
    {
        public SumFunction() { }

        public SumFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
