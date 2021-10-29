using System.Collections.Generic;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IQueryFieldProvider
    {
        IEnumerable<string> GetSelectFields(IEnumerable<string> querySelectFields);
        string GetDatabaseFieldName(string queryFieldName);
        IEnumerable<string> GetAllFields();
        bool ValidateExpression(IQueryExpression expression);
    }
}
