using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Core.Functions;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer.FunctionParsers
{
    public class CoalesceFunctionParser : IFunctionParser
    {
        public bool TryParse(IQueryExpressionFunction function, out string sqlExpression)
        {
            if (function is CoalesceFunction f)
            {
                sqlExpression = $"COALESCE({FieldNameAsString(f)}{InnerExpressionsAsString(f)})";
                return true;
            }

            sqlExpression = string.Empty;
            return false;
        }

        private static string InnerExpressionsAsString(CoalesceFunction instance)
            => string.Join(", ", instance.InnerExpressions.Select(x => x.GetSqlExpression()));

        private static string FieldNameAsString(CoalesceFunction instance)
            => string.IsNullOrWhiteSpace(instance.FieldName)
                ? string.Empty
                : "{0}, ";
    }
}
