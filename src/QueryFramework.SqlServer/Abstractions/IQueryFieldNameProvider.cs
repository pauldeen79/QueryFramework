using System.Collections.Generic;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IQueryFieldNameProvider
    {
        IEnumerable<string> GetSelectFields(IEnumerable<string> querySelectFields);
    }
}
