using System;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Functions
{
    public sealed class DateFunction : IQueryExpressionFunction
    {
        public DateFunction(DateTime date) => Date = date;

        public IQueryExpressionFunction? InnerFunction => null;

        public DateTime Date { get; }
    }
}
