using System.Linq;
using CrossCutting.Common.Extensions;
using ModelFramework.Objects.Builders;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace QueryFramework.CodeGeneration.CodeGenerationProviders
{
    public class AbstractionsExtensionsBuilders : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "QueryFramework.Abstractions\\Extensions";

        public override string DefaultFileName => "Builders.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        protected override string SetMethodNameFormatString => "With{0}";

        public override object CreateModel()
            => GetImmutableBuilderExtensionClasses
            (
                GetModels(),
                "QueryFramework.Core",
                "QueryFramework.Core.Builders"
            )
            .Select
            (
                x => new ClassBuilder(x)
                      .WithNamespace("QueryFramework.Abstractions.Extensions")
                      .Chain
                      (
                        x => x.Methods.ForEach(y => y.AddGenericTypeArguments("T").AddGenericTypeArgumentConstraints($"where T : QueryFramework.Abstractions.Builders.I{y.TypeName}"))
                      )
                      .Chain
                      (
                        x => x.Methods.ForEach(y => y.WithTypeName("T")
                                                     .Chain(z => z.Parameters.First().WithTypeName(z.TypeName)))
                      )
                      .Build()
            ).ToArray();
    }
}
