﻿using System.Linq;
using CrossCutting.Common.Extensions;
using ModelFramework.Objects.Builders;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace QueryFramework.CodeGeneration.CodeGenerationProviders
{
    public class CoreBuilders : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "QueryFramework.Core\\Builders";

        public override string DefaultFileName => "Builders.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => GetImmutableBuilderClasses
            (
                GetModels().Where(x => x.Name != "IQueryExpressionFunction").ToArray(),
                "QueryFramework.Core",
                "QueryFramework.Core.Builders",
                "QueryFramework.Abstractions.Builders.I{0}"
            ).Select(x => new ClassBuilder(x).Chain(y => y.Methods.RemoveAll(z => z.Static)).Build())
            .ToArray();
    }
}