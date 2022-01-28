using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record LeftFunction : IQueryExpressionFunction
    {
        public LeftFunction(int length)
            => Length = length;

        public LeftFunction(int length, IQueryExpressionFunction? innerFunction)
        {
            Length = length;
            InnerFunction = innerFunction;
        }

        public int Length { get; }
        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new LeftFunctionBuilder().WithLength(Length).WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class LeftFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public int Length { get; set; }
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public LeftFunctionBuilder WithLength(int length)
            => this.Chain(x => x.Length = length);

        public IQueryExpressionFunction Build()
            => new LeftFunction(Length, InnerFunction?.Build());
    }
}
