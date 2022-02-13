namespace ExpressionFramework.Core.Tests.TestHelpers;

public class ExpressionEvaluatorMock : IExpressionEvaluator
{
    public Func<object?, IExpression, Tuple<bool, object?>> Delegate { get; set; }
        = new Func<object?, IExpression, Tuple<bool, object?>>((_, _) => new Tuple<bool, object?>(default, default));

    public bool TryEvaluate(object? item, IExpression expression, out object? result)
    {
        var x = Delegate.Invoke(item, expression);

        result = x.Item2;
        return x.Item1;
    }
}
