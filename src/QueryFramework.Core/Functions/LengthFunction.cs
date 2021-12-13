using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record LengthFunction : IQueryExpressionFunction
    {
        public LengthFunction() { }

        public LengthFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
