using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record YearFunction : IQueryExpressionFunction
    {
        public YearFunction() { }
        
        public YearFunction(IQueryExpressionFunction? innerFunction)
            => InnerFunction = innerFunction;

        public IQueryExpressionFunction? InnerFunction { get; }
        
        public IQueryExpressionFunctionBuilder ToBuilder()
            => new YearFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class YearFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public IQueryExpressionFunction Build()
            => new YearFunction(InnerFunction?.Build());
    }
}
