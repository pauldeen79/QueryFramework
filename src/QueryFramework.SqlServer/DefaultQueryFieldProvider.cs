using System.Collections.Generic;
using System.Linq;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer
{
    /// <summary>
    /// Default query field provider, which has enough functionality for single-entity queries. Getting all select fields (SelectAll) is not supported.
    /// </summary>
    public class DefaultQueryFieldProvider : IQueryFieldProvider
    {
        public IEnumerable<string> GetAllFields()
            => Enumerable.Empty<string>();

        public string? GetDatabaseFieldName(string queryFieldName)
            => queryFieldName;
    }
}
