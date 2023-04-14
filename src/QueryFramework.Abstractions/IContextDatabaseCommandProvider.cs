namespace QueryFramework.Abstractions;

public interface IContextDatabaseCommandProvider<in TSource>: IDatabaseCommandProvider<TSource>
{
    IDatabaseCommand Create(TSource source, DatabaseOperation operation, object? context);
}
