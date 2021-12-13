using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record UpperFunction : IQueryExpressionFunction
    {
        public UpperFunction() { }

        public UpperFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
