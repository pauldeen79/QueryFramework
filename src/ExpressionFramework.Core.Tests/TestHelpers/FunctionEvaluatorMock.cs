namespace ExpressionFramework.Core.Tests.TestHelpers;

public class FunctionEvaluatorMock : IFunctionEvaluator
{
    public Func<IExpressionFunction, object?, IExpressionEvaluator, Tuple<bool, object?>> Delegate { get; set; }
        = new Func<IExpressionFunction, object?, IExpressionEvaluator, Tuple<bool, object?>>((_, _, _) => new Tuple<bool, object?>(default, default));

    public bool TryEvaluate(IExpressionFunction function, object? value, IExpressionEvaluator evaluator, out object? result)
    {
        var x = Delegate.Invoke(function, value, evaluator);

        result = x.Item2;
        return x.Item1;
    }
}
