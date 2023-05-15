﻿namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsCoreExtensionsBuilders : QueryFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Abstractions}/Extensions";

    protected override string SetMethodNameFormatString => "With{0}";

    public override object CreateModel()
        => GetImmutableBuilderExtensionClasses
        (
            GetCoreModels(),
            Constants.Namespaces.Abstractions,
            Constants.Namespaces.AbstractionsExtensions,
            Constants.Namespaces.AbstractionsBuilders
        );
}
