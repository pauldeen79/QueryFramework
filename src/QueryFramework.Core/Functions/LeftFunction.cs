﻿namespace QueryFramework.Core.Functions;

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
