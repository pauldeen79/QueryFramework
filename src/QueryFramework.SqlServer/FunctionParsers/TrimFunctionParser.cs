using QueryFramework.Abstractions;
using QueryFramework.Core.Functions;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.FunctionParsers
{
    public class TrimFunctionParser : IFunctionParser
    {
        public bool TryParse(IQueryExpressionFunction function, out string sqlExpression)
        {
            if (function is TrimFunction f)
            {
                sqlExpression = "TRIM({0})";
                return true;
            }

            sqlExpression = string.Empty;
            return false;
        }
    }
}
