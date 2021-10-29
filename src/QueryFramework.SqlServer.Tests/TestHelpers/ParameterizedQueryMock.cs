using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.SqlServer.Tests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    internal class ParameterizedQueryMock : IParameterizedQuery
    {
        public ParameterizedQueryMock(IEnumerable<IQueryParameter> parameters)
        {
            Conditions = new List<IQueryCondition>();
            OrderByFields = new List<IQuerySortOrder>();
            Parameters = parameters.ToList();
        }

        public IReadOnlyCollection<IQueryParameter> Parameters { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public IReadOnlyCollection<IQueryCondition> Conditions { get; set; }

        public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }
    }
}
