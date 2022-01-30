using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record SumFunction : IQueryExpressionFunction
    {
        public SumFunction() { }

        public SumFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new SumFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class SumFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new SumFunction(InnerFunction?.Build());
    }
}
