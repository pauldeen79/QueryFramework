using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Extensions;
namespace QueryFramework.Core.Functions
{
    public record CountFunction : IQueryExpressionFunction
    {
        public CountFunction() { }

        public CountFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new CountFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class CountFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new CountFunction(InnerFunction?.Build());
    }
}
