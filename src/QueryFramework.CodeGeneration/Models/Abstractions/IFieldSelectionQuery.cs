﻿namespace QueryFramework.CodeGeneration.Models.Abstractions;

internal interface IFieldSelectionQuery : IQuery
{
    bool Distinct { get; }
    bool GetAllFields { get; }
    [Required] IReadOnlyCollection<string> FieldNames { get; }
}
