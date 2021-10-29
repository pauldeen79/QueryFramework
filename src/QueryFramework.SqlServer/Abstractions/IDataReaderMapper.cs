using System.Data;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IDataReaderMapper<out T>
    {
        T Map(IDataReader reader);
    }
}
