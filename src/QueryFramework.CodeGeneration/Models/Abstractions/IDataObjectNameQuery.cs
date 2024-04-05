﻿namespace QueryFramework.CodeGeneration.Models.Abstractions;

internal interface IDataObjectNameQuery : IQuery
{
    [Required(AllowEmptyStrings = true)] string DataObjectName { get; }
}
