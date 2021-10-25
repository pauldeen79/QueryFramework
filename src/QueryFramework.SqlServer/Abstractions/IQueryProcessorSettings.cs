using System;
using System.Collections.Generic;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IQueryProcessorSettings
    {
        string TableName { get; }
        string Fields { get; }
        string DefaultOrderBy { get; }
        string DefaultWhere { get; }
        int? OverrideLimit { get; }
        bool ValidateFieldNames { get; }
        int InitialParameterNumber { get; }
        string FieldPrefix { get; }
        IEnumerable<string> SkipFields { get; }
        Func<string, string> GetFieldNameDelegate { get; }
        Func<IEnumerable<string>> GetAllFieldsDelegate { get; }
        Func<IQueryExpression, bool> ExpressionValidationDelegate { get; }
    }
}
