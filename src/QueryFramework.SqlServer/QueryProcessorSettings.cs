using QueryFramework.SqlServer.Abstractions;

namespace QueryFramework.SqlServer
{
    public class QueryProcessorSettings : IQueryProcessorSettings
    {
        public string TableName { get; }
        public string Fields { get; }
        public string DefaultOrderBy { get; }
        public string DefaultWhere { get; }
        public int? OverridePageSize { get; }
        public bool ValidateFieldNames { get; }
        public int InitialParameterNumber { get; }

        public QueryProcessorSettings(string tableName = "",
                                      string fields = "",
                                      string defaultOrderBy = "",
                                      string defaultWhere = "",
                                      int? overridePageSize = null,
                                      bool validateFieldNames = true)
        {
            TableName = tableName;
            Fields = fields;
            DefaultOrderBy = defaultOrderBy;
            DefaultWhere = defaultWhere;
            OverridePageSize = overridePageSize;
            ValidateFieldNames = validateFieldNames;
            InitialParameterNumber = 0;
        }
    }
}
