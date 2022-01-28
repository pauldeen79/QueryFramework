using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record UpperFunction : IQueryExpressionFunction
    {
        public UpperFunction() { }

        public UpperFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new UpperFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class UpperFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new UpperFunction(InnerFunction?.Build());
    }
}
