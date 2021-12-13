using System;
using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.FunctionParsers;

namespace QueryFramework.SqlServer.Extensions
{
    public static class QueryExpressionFunctionExtensions
    {
        private readonly static IFunctionParser[] _functionParsers = new IFunctionParser[]
        {
            new CoalesceFunctionParser(),
            new CountFunctionFunctionParser(),
            new DayFunctionFunctionParser(),
            new LeftFunctionParser(),
            new LengthFunctionParser(),
            new LowerFunctionParser(),
            new MonthFunctionParser(),
            new RightFunctionParser(),
            new SumFunctionParser(),
            new TrimFunctionParser(),
            new UpperFunctionParser(),
            new YearFunctionParser()
        };

        public static string GetSqlExpression(this IQueryExpressionFunction function)
        {
            var inner = function.InnerFunction != null
                ? function.InnerFunction.GetSqlExpression()
                : string.Empty;

            foreach (var parser in _functionParsers)
            {
                if (parser.TryParse(function, out var sqlExpression))
                {
                    return Combine(sqlExpression, inner);
                }
            }

            throw new ArgumentException($"Unsupported function: {function.GetType().Name}", nameof(function));
        }

        private static string Combine(string sqlExpression, string inner)
            => inner.Length > 0
                ? string.Format(sqlExpression, inner)
                : sqlExpression;
    }
}
