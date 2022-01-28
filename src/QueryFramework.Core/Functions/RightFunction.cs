using CrossCutting.Common.Extensions;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Builders;
using QueryFramework.Core.Extensions;

namespace QueryFramework.Core.Functions
{
    public record RightFunction : IQueryExpressionFunction
    {
        public RightFunction(int length)
            => Length = length;

        public RightFunction(int length, IQueryExpressionFunction? innerFunction)
        {
            Length = length;
            InnerFunction = innerFunction;
        }

        public int Length { get; }
        public IQueryExpressionFunction? InnerFunction { get; }

        public IQueryExpressionFunctionBuilder ToBuilder()
            => new RightFunctionBuilder().WithLength(Length).WithInnerFunction(InnerFunction?.ToBuilder());
    }

    public class RightFunctionBuilder : IQueryExpressionFunctionBuilder
    {
        public int Length { get; set; }
        public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

        public RightFunctionBuilder WithLength(int length)
            => this.Chain(x => x.Length = length);

        public IQueryExpressionFunction Build()
            => new RightFunction(Length, InnerFunction?.Build());
    }
}
