﻿namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => $"{Constants.Namespaces.Core}/Builders";

    public override object CreateModel()
        => GetImmutableBuilderClasses
        (
            GetCoreModels(),
            Constants.Namespaces.Core,
            Constants.Namespaces.CoreBuilders,
            $"{Constants.Namespaces.AbstractionsBuilders}.I{{0}}"
        )
        .Cast<IClass>()
        .Select(x => new ClassBuilder(x).With(y => y.Methods.RemoveAll(z => z.Static)).Build())
        .ToArray();
}