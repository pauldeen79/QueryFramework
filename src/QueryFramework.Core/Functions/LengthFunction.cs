using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record LengthFunction : IQueryExpressionFunction
    {
        public LengthFunction() { }

        public LengthFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new LengthFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class LengthFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new LengthFunction(InnerFunction?.Build());
    }
}
