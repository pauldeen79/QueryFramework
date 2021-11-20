using System.Collections.Generic;
using System.Linq;
using QueryFramework.Abstractions;
using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer
{
    /// <summary>
    /// Default query field provider, which has enough functionality for single-entity queries. Getting all select fields (SelectAll) is not supported.
    /// </summary>
    public class DefaultQueryFieldProvider : IQueryFieldProvider
    {
        public virtual IEnumerable<string> GetAllFields()
            => Enumerable.Empty<string>();

        public string? GetDatabaseFieldName(string queryFieldName)
            => queryFieldName;

        public IEnumerable<string> GetSelectFields(IEnumerable<string> querySelectFields)
            => querySelectFields;

        public bool ValidateExpression(IQueryExpression expression)
            => true;
    }
}
