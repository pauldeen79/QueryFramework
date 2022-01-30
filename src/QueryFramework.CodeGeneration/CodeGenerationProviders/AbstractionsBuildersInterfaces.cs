using System.Linq;
using CrossCutting.Common.Extensions;
using ModelFramework.Objects.Builders;
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
            .Select(x => new ClassBuilder(x).Chain(y => y.Methods.RemoveAll(y => y.Name.StartsWith("With"))).Build())
            .Select
            (
                x => x.ToInterfaceBuilder()
                      .WithPartial()
                      .WithNamespace("QueryFramework.Abstractions.Builders")
                      .WithName($"I{x.Name}")
                      .Chain(x => x.Interfaces.Clear())
                      .Build()
            )
            .ToArray();
    }
}
