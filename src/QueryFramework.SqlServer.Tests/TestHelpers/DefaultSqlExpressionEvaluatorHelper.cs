namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal static class DefaultSqlExpressionEvaluatorHelper
{
    internal static void UseRealSqlExpressionEvaluator(Mock<ISqlExpressionEvaluator> evaluatorMock, ParameterBag parameterBag)
    {
        var evaluator = new DefaultSqlExpressionEvaluator
        (
            new ISqlExpressionEvaluatorProvider[] { new FieldExpressionEvaluatorProvider(),
                                                    new ConstantExpressionEvaluatorProvider() },
            Enumerable.Empty<IFunctionParser>()
        );
        evaluatorMock.Setup(x => x.GetSqlExpression(It.IsAny<Expression>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<ParameterBag>(), It.IsAny<object?>()))
                     .Returns<Expression, IQueryFieldInfo, ParameterBag, object?>((expression, fieldInfo, _, context)
                      => evaluator.GetSqlExpression(expression, fieldInfo, parameterBag, context));
        evaluatorMock.Setup(x => x.GetLengthExpression(It.IsAny<Expression>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<object?>()))
                     .Returns<Expression, IQueryFieldInfo, object?>((expression, fieldInfo, context)
                      => evaluator.GetLengthExpression(expression, fieldInfo, context));
    }
}
