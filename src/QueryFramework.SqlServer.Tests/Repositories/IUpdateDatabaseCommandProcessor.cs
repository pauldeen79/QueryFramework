using CrossCutting.Data.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    public interface IUpdateDatabaseCommandProcessor<T> : IDatabaseCommandProcessor<T> where T: class
    {
    }
}
