using System.Linq;
using CrossCutting.Common.Extensions;
using ModelFramework.Objects.Extensions;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace QueryFramework.CodeGeneration.CodeGenerationProviders
{
    public class AbstractionsBuildersInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "QueryFramework.Abstractions\\Builders";

        public override string DefaultFileName => "Interfaces.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        public override object CreateModel()
            => GetImmutableBuilderClasses
            (
                GetModels(),
                "QueryFramework.Core",
                "QueryFramework.Core.Builders"
            )
            .Select
            (
                x => x.ToInterfaceBuilder()
                      .WithPartial()
                      .WithNamespace("QueryFramework.Abstractions.Builders")
                      .WithName($"I{x.Name}")
                      .Chain(x => x.Methods.RemoveAll(y => y.Name != "Build"))
                      .Build()
            )
            .ToArray();
    }
}
