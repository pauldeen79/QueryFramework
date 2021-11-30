using CrossCutting.Data.Abstractions;

namespace QueryFramework.SqlServer
{
    public class PagedDatabaseEntityRetrieverSettings : IPagedDatabaseEntityRetrieverSettings
    {
        public string TableName { get; }
        public string Fields { get; }
        public string DefaultOrderBy { get; }
        public string DefaultWhere { get; }
        public int? OverridePageSize { get; }
        
        public PagedDatabaseEntityRetrieverSettings(string tableName = "",
                                                    string fields = "",
                                                    string defaultOrderBy = "",
                                                    string defaultWhere = "",
                                                    int? overridePageSize = null)
        {
            TableName = tableName;
            Fields = fields;
            DefaultOrderBy = defaultOrderBy;
            DefaultWhere = defaultWhere;
            OverridePageSize = overridePageSize;
        }
    }
}
