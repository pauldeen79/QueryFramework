namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal static class DefaultSqlExpressionEvaluatorHelper
{
    internal static void UseRealSqlExpressionEvaluator(ISqlExpressionEvaluator evaluatorMock, ParameterBag parameterBag)
    {
        var evaluator = new DefaultSqlExpressionEvaluator
        (
            [ new FieldExpressionEvaluatorProvider(), new ConstantExpressionEvaluatorProvider() ],
            Enumerable.Empty<IFunctionParser>()
        );
        evaluatorMock
            .GetSqlExpression(Arg.Any<IQuery>(), Arg.Any<Expression>(), Arg.Any<IQueryFieldInfo>(), Arg.Any<ParameterBag>())
            .Returns(x => evaluator.GetSqlExpression(x.ArgAt<IQuery>(0), x.ArgAt<Expression>(1), x.ArgAt<IQueryFieldInfo>(2), parameterBag));
        evaluatorMock
            .GetLengthExpression(Arg.Any<IQuery>(), Arg.Any<Expression>(), Arg.Any<IQueryFieldInfo>())
            .Returns(x => evaluator.GetLengthExpression(x.ArgAt<IQuery>(0), x.ArgAt<Expression>(1), x.ArgAt<IQueryFieldInfo>(2)));
    }
}
