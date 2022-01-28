using System.Collections.Generic;
using System.Linq;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace QueryFramework.CodeGeneration.CodeGenerationProviders
{
    public class AbstractionsInterfaces : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
    {
        public override string Path => "QueryFramework.Abstractions";

        public override string DefaultFileName => "Interfaces.generated.cs";

        public override bool RecurseOnDeleteGeneratedFiles => false;

        //TODO: Move to ModelFramework as "ToInterface" method, or make it possible that you can convert a type to IInterface instead of IClass
        public override object CreateModel()
            => GetModels().Select(x => new InterfaceBuilder()
                .WithName(x.Name)
                .WithNamespace(x.Namespace)
                .WithPartial()
                .WithVisibility(x.Visibility)
                .AddAttributes(x.Attributes)
                .AddMetadata(x.Metadata)
                .AddMethods(x.Methods)
                .AddProperties(x.Properties)
                .Build());

        protected override IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
            => Enumerable.Empty<ClassMethodBuilder>();
    }
}
