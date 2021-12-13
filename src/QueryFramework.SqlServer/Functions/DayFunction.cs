using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.Functions
{
    public record DayFunction : IQueryExpressionFunction
    {
        public DayFunction() { }

        public DayFunction(IQueryExpressionFunction? innerFunction) => InnerFunction = innerFunction;

        public string Expression => InnerFunction.GetExpression("DAY({0})");

        public IQueryExpressionFunction? InnerFunction { get; }
    }
}
