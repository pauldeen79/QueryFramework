using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IFunctionParser
    {
        bool TryParse(IQueryExpressionFunction function, out string sqlExpression);
    }
}
