using ExpressionFramework.Domain.Builders.Evaluatables;
using Pipelines = ClassFramework.Pipelines;

namespace QueryFramework.CodeGeneration2.CodeGenerationProviders;

public abstract class QueryFrameworkCSharpClassBase : CsharpClassGeneratorPipelineCodeGenerationProviderBase
{
    protected QueryFrameworkCSharpClassBase(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, builderExtensionPipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type EntityCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type EntityConcreteCollectionType => typeof(List<>);
    protected override Type BuilderCollectionType => typeof(List<>);

    protected override string ProjectName => "QueryFramework";
    protected override string CodeGenerationRootNamespace => "QueryFramework.CodeGeneration2";
    protected override string CoreNamespace => "QueryFramework.Core";
    protected override bool CopyAttributes => true;
    protected override bool CopyInterfaces => true;

    protected override IEnumerable<NamespaceMappingBuilder> CreateNamespaceMappings()
    {
        // From models to domain entities
        yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CodeGenerationRootNamespace}.Models").WithTargetNamespace(CoreNamespace);
        yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CodeGenerationRootNamespace}.Models.Domains").WithTargetNamespace($"{ProjectName}.Abstractions.Domains");
        yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CodeGenerationRootNamespace}.Models.Abstractions").WithTargetNamespace($"{ProjectName}.Abstractions");

        // From domain entities to builders
        yield return new NamespaceMappingBuilder().WithSourceNamespace($"{ProjectName}.Abstractions").WithTargetNamespace($"{ProjectName}.Abstractions")
            .AddMetadata
            (
                new MetadataBuilder().WithValue($"{ProjectName}.Abstractions.Builders").WithName(Pipelines.MetadataNames.CustomBuilderInterfaceNamespace),
                new MetadataBuilder().WithValue("{TypeName.ClassName}Builder").WithName(Pipelines.MetadataNames.CustomBuilderInterfaceName),
                new MetadataBuilder().WithValue($"{ProjectName}.Abstractions.Builders").WithName(Pipelines.MetadataNames.CustomBuilderParentTypeNamespace),
                new MetadataBuilder().WithValue("{ParentTypeName.ClassName}Builder").WithName(Pipelines.MetadataNames.CustomBuilderParentTypeName)
            );

        foreach (var entityClassName in GetPureAbstractModels().Select(x => x.GetEntityClassName().ReplaceSuffix("Base", string.Empty, StringComparison.Ordinal)))
        {
            yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CodeGenerationRootNamespace}.Models.{entityClassName}s").WithTargetNamespace($"{CoreNamespace}.{entityClassName}s");
            yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CoreNamespace}.{entityClassName}s").WithTargetNamespace($"{CoreNamespace}.{entityClassName}s");
        }
    }

    protected override IEnumerable<TypenameMappingBuilder> CreateTypenameMappings()
        => base.CreateTypenameMappings().Concat(
        [
            new TypenameMappingBuilder()
                .WithSourceTypeName(typeof(ComposedEvaluatable).FullName!)
                .WithTargetTypeName(typeof(ComposedEvaluatable).FullName!)
                .AddMetadata
                (
                    new MetadataBuilder().WithValue(typeof(ComposedEvaluatableBuilder).Namespace).WithName(Pipelines.MetadataNames.CustomBuilderNamespace),
                    new MetadataBuilder().WithValue("{TypeName.ClassName}Builder").WithName(Pipelines.MetadataNames.CustomBuilderName),
                    //TODO: Refactor so the custom builder constructor initialize expression can be set for both single and collection properties
                    new MetadataBuilder().WithValue($"{{BuilderMemberName}} = new {typeof(ComposedEvaluatableBuilder).FullName}(source.{{Name}})").WithName(Pipelines.MetadataNames.CustomBuilderConstructorInitializeExpression),
                    //TODO: Refactor so the custom builder default value expression can be set for both single and collection properties
                    new MetadataBuilder().WithValue(new Literal($"default({typeof(ComposedEvaluatableBuilder).FullName})!", null)).WithName(Pipelines.MetadataNames.CustomBuilderDefaultValue),
                    new MetadataBuilder().WithValue("[Name][NullableSuffix].BuildTyped()").WithName(Pipelines.MetadataNames.CustomBuilderMethodParameterExpression)
                )
        ]);
}
