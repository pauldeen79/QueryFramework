namespace ExpressionFramework.Core.Tests.TestHelpers;

public class ExpressionEvaluatorMock : IExpressionEvaluator
{
    public Func<object?, IExpression, IExpressionEvaluatorCallback, Tuple<bool, object?>> Delegate { get; set; }
        = new Func<object?, IExpression, IExpressionEvaluatorCallback, Tuple<bool, object?>>((_, _, _) => new Tuple<bool, object?>(default, default));

    public bool TryEvaluate(object? item, IExpression expression, IExpressionEvaluatorCallback callback, out object? result)
    {
        var x = Delegate.Invoke(item, expression, callback);

        result = x.Item2;
        return x.Item1;
    }
}
