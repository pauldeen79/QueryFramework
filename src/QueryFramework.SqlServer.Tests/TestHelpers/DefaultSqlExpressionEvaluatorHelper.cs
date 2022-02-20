namespace QueryFramework.SqlServer.Tests.TestHelpers;

internal static class DefaultSqlExpressionEvaluatorHelper
{
    internal static void UseRealSqlExpressionEvaluator(Mock<ISqlExpressionEvaluator> evaluatorMock)
    {
        var evaluator = new DefaultSqlExpressionEvaluator
        (
            new ISqlExpressionEvaluatorProvider[] { new FieldExpressionEvaluatorProvider(),
                                                    new ConstantExpressionEvaluatorProvider() },
            Enumerable.Empty<IFunctionParser>()
        );
        evaluatorMock.Setup(x => x.GetSqlExpression(It.IsAny<IExpression>(), It.IsAny<IQueryFieldInfo>(), It.IsAny<int>()))
                     .Returns<IExpression, IQueryFieldInfo, int>((expression, fieldInfo, paramCounter)
                      => evaluator.GetSqlExpression(expression, fieldInfo, paramCounter));
        evaluatorMock.Setup(x => x.GetLengthExpression(It.IsAny<IExpression>(), It.IsAny<IQueryFieldInfo>()))
                     .Returns<IExpression, IQueryFieldInfo>((expression, fieldInfo)
                      => evaluator.GetLengthExpression(expression, fieldInfo));
    }
}
