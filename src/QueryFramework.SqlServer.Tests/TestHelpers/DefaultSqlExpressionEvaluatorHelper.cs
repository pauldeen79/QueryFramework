namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal static class DefaultSqlExpressionEvaluatorHelper
{
    internal static void UseRealSqlExpressionEvaluator(ISqlExpressionEvaluator evaluatorMock, ParameterBag parameterBag)
    {
        var evaluator = new DefaultSqlExpressionEvaluator
        (
            new ISqlExpressionEvaluatorProvider[] { new FieldExpressionEvaluatorProvider(),
                                                    new ConstantExpressionEvaluatorProvider() },
            Enumerable.Empty<IFunctionParser>()
        );
        evaluatorMock.GetSqlExpression(Arg.Any<Expression>(), Arg.Any<IQueryFieldInfo>(), Arg.Any<ParameterBag>(), Arg.Any<object?>())
                     .Returns(x
                      => evaluator.GetSqlExpression(x.ArgAt<Expression>(0), x.ArgAt<IQueryFieldInfo>(1), parameterBag, x.ArgAt<object?>(3)));
        evaluatorMock.GetLengthExpression(Arg.Any<Expression>(), Arg.Any<IQueryFieldInfo>(), Arg.Any<object?>())
                     .Returns(x
                      => evaluator.GetLengthExpression(x.ArgAt<Expression>(0), x.ArgAt<IQueryFieldInfo>(1), x.ArgAt<object?>(2)));
    }
}
