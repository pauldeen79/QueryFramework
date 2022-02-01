using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Abstractions.Extensions;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record TrimFunction : IQueryExpressionFunction
    {
        public TrimFunction() { }

        public TrimFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new TrimFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class TrimFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new TrimFunction(InnerFunction?.Build());
    }
}
