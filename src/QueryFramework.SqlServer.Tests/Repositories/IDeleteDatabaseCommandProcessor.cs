using CrossCutting.Data.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    public interface IDeleteDatabaseCommandProcessor<T> : IDatabaseCommandProcessor<T> where T: class
    {
    }
}
