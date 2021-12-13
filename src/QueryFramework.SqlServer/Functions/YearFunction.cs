using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record YearFunction : IQueryExpressionFunction
    {
        public YearFunction() { }
        
        public YearFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("YEAR({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
