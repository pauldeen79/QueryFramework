namespace QueryFramework.CodeGeneration2.Models.Abstractions;

internal interface IDataObjectNameQuery : IQuery
{
    [Required(AllowEmptyStrings = true)] string DataObjectName { get; }
}
