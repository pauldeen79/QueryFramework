using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record DayFunction : IQueryExpressionFunction
    {
        public DayFunction() { }

        public DayFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new DayFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class DayFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new DayFunction(InnerFunction?.Build());
    }
}
