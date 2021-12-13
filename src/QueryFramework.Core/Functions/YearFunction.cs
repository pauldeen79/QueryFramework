using QueryFramework.Abstractions;

namespace QueryFramework.Core.Functions
{
    public record YearFunction : IQueryExpressionFunction
    {
        public YearFunction() { }
        
        public YearFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
