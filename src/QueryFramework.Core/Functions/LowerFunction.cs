using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record LowerFunction : IQueryExpressionFunction
    {
        public LowerFunction() { }

        public LowerFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new LowerFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class LowerFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new LowerFunction(InnerFunction?.Build());
    }
}
