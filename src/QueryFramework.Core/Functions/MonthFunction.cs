using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record MonthFunction : IQueryExpressionFunction
    {
        public MonthFunction() { }

        public MonthFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new MonthFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class MonthFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new MonthFunction(InnerFunction?.Build());
    }
}
