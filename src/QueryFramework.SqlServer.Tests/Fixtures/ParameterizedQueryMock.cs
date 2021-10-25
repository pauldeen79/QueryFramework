using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.SqlServer.Tests.Fixtures
{
    [ExcludeFromCodeCoverage]
    internal class ParameterizedQueryMock : IParameterizedQuery
    {
        public IReadOnlyCollection<IQueryParameter> Parameters { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public IReadOnlyCollection<IQueryCondition> Conditions { get; set; }

        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }

        public ParameterizedQueryMock()
        {
            Conditions = new List<IQueryCondition>();
            OrderByFields = new List<IQuerySortOrder>();
            Parameters = new List<IQueryParameter>();
        }
    }
}
