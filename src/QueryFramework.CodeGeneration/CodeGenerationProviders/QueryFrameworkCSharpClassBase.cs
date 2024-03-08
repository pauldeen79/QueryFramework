namespace QueryFramework.CodeGeneration2.CodeGenerationProviders;

public abstract class QueryFrameworkCSharpClassBase : CsharpClassGeneratorPipelineCodeGenerationProviderBase
{
    protected QueryFrameworkCSharpClassBase(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, builderExtensionPipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    private const string TypeNameDotClassNameBuilder = "{TypeName.ClassName}Builder";

    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type EntityCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type EntityConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override Type BuilderCollectionType => typeof(List<>);

    protected override string ProjectName => "QueryFramework";
    protected override string CodeGenerationRootNamespace => "QueryFramework.CodeGeneration2";
    protected override string CoreNamespace => "QueryFramework.Core";
    protected override bool CopyAttributes => true;
    protected override bool CopyInterfaces => true;
    protected override bool InheritFromInterfaces => true;
    protected override bool CopyMethods => true;
    protected override bool CreateRecord => true;

    protected override IEnumerable<TypenameMappingBuilder> CreateTypenameMappings()
        => base.CreateTypenameMappings().Concat(
        [
            new TypenameMappingBuilder()
                .WithSourceTypeName(typeof(ComposedEvaluatable).FullName!)
                .WithTargetTypeName(typeof(ComposedEvaluatable).FullName!)
                .AddMetadata
                (
                    new MetadataBuilder().WithValue(typeof(ComposedEvaluatableBuilder).Namespace).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderNamespace),
                    new MetadataBuilder().WithValue(TypeNameDotClassNameBuilder).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderName),
                    new MetadataBuilder().WithValue($"new {typeof(ComposedEvaluatableBuilder).FullName}(source.{{Name}})").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderConstructorInitializeExpression),
                    new MetadataBuilder().WithValue(new Literal($"new {typeof(ComposedEvaluatableBuilder).FullName}()", null)).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderDefaultValue),
                    new MetadataBuilder().WithValue("[Name][NullableSuffix].BuildTyped()").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderMethodParameterExpression)
                ),
            new TypenameMappingBuilder()
                .WithSourceTypeName(typeof(Expression).FullName!)
                .WithTargetTypeName(typeof(Expression).FullName!)
                .AddMetadata
                (
                    new MetadataBuilder().WithValue(typeof(ExpressionBuilder).Namespace).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderNamespace),
                    new MetadataBuilder().WithValue(TypeNameDotClassNameBuilder).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderName),
                    new MetadataBuilder().WithValue($"{typeof(ExpressionBuilderFactory).FullName}.Create(source.[Name])").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderConstructorInitializeExpression),
                    new MetadataBuilder().WithValue(new Literal($"default({typeof(ExpressionBuilder).FullName})!", null)).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderDefaultValue),
                    new MetadataBuilder().WithValue($"[Name][NullableSuffix].Build()").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderMethodParameterExpression)
                ),
        ]).Concat(
            GetType().Assembly.GetTypes()
                .Where(x => x.IsInterface
                    && x.Namespace == $"{CodeGenerationRootNamespace}.Models.Abstractions"
                    && !SkipNamespaceOnTypenameMappings(x.Namespace)
                    && x.FullName is not null)
                .SelectMany(x =>
                    new[]
                    {
                        new TypenameMappingBuilder().WithSourceTypeName(x.FullName!).WithTargetTypeName($"{ProjectName}.Abstractions.{x.Name}"),
                        new TypenameMappingBuilder().WithSourceTypeName($"{ProjectName}.Abstractions.{x.Name.Substring(1)}").WithTargetTypeName($"{ProjectName}.Abstractions.{x.Name}"), // hacking
                        new TypenameMappingBuilder().WithSourceTypeName($"{ProjectName}.Abstractions.{x.Name}").WithTargetTypeName($"{ProjectName}.Abstractions.{x.Name}")
                            .AddMetadata
                            (
                                new MetadataBuilder().WithValue($"{ProjectName}.Abstractions.Builders").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderNamespace),
                                new MetadataBuilder().WithValue(TypeNameDotClassNameBuilder).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderName),
                                new MetadataBuilder().WithValue($"{ProjectName}.Abstractions.Builders").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceNamespace),
                                new MetadataBuilder().WithValue(TypeNameDotClassNameBuilder).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderInterfaceName),
                                //new MetadataBuilder().WithValue($"new {ProjectName}.Core.Builders.{x.Name.Substring(1)}Builder([Name])").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderSourceExpression),
                                new MetadataBuilder().WithValue("[Name][NullableSuffix].ToBuilder()").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderSourceExpression),
                                new MetadataBuilder().WithValue("[Name][NullableSuffix].Build()").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderMethodParameterExpression),
                                new MetadataBuilder().WithName(ClassFramework.Pipelines.MetadataNames.CustomEntityInterfaceTypeName).WithValue($"{ProjectName}.Abstractions.I{x.GetEntityClassName()}")
                            )
                    })
        );
}
