﻿namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

public class CoreRecords : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => "QueryFramework.Core";

    public override string DefaultFileName => "Entities.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableClasses(GetModels().Where(x => x.Name != "IQueryExpressionFunction").ToArray(), "QueryFramework.Core");
}
