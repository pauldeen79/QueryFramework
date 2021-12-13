using QueryFramework.Abstractions;
using QueryFramework.Core.Functions;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.FunctionParsers
{
    public class DayFunctionFunctionParser : IFunctionParser
    {
        public bool TryParse(IQueryExpressionFunction function, out string sqlExpression)
        {
            if (function is DayFunction f)
            {
                sqlExpression = "DAY({0})";
                return true;
            }

            sqlExpression = string.Empty;
            return false;
        }
    }
}
