using CrossCutting.Data.Abstractions;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IQueryProcessorSettings : IPagedDatabaseEntityRetrieverSettings
    {
        bool ValidateFieldNames { get; }
        int InitialParameterNumber { get; }
   }
}
