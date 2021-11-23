using CrossCutting.Data.Abstractions;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IQueryProcessorSettings : IDatabaseEntityRetrieverSettings
    {
        int? OverrideLimit { get; }
        bool ValidateFieldNames { get; }
        int InitialParameterNumber { get; }
   }
}
