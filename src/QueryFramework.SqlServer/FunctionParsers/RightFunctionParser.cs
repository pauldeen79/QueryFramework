using QueryFramework.Abstractions;
using QueryFramework.Core.Functions;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer.FunctionParsers
{
    public class RightFunctionParser : IFunctionParser
    {
        public bool TryParse(IQueryExpressionFunction function, out string sqlExpression)
        {
            if (function is RightFunction f)
            {
                sqlExpression = $"RIGHT({{0}}, {f.Length})";
                return true;
            }

            sqlExpression = string.Empty;
            return false;
        }
    }
}
