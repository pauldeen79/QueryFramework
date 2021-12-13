using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Functions;

namespace QueryFramework.SqlServer.FunctionParsers
{
    public class DateFunctionParser : IFunctionParser
    {
        public bool TryParse(IQueryExpressionFunction function, out string sqlExpression)
        {
            if (function is DateFunction f)
            {
                sqlExpression = $"'{f.Date.ToString("yyyy-MM-dd HH:mm:ss")}'";
                return true;
            }

            sqlExpression = string.Empty;
            return false;
        }
    }
}
