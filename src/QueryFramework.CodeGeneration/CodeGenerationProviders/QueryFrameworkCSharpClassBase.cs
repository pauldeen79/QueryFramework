using ClassFramework.Domain.Extensions;
using CrossCutting.Common.Abstractions;

namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

public abstract class QueryFrameworkCSharpClassBase : CsharpClassGeneratorPipelineCodeGenerationProviderBase
{
    private const string TypeNameDotClassNameBuilder = "{NoGenerics(ClassName(property.TypeName))}Builder";

    protected QueryFrameworkCSharpClassBase(IPipelineService pipelineService) : base(pipelineService)
    {
    }

    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type EntityCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type EntityConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override Type BuilderCollectionType => typeof(List<>);

    protected override string ProjectName => "QueryFramework";
    protected override string CodeGenerationRootNamespace => "QueryFramework.CodeGeneration";
    protected override string CoreNamespace => "QueryFramework.Core";
    protected override bool CopyAttributes => true;
    protected override bool CopyInterfaces => true;
    protected override bool InheritFromInterfaces => true;
    protected override bool CopyMethods => true;
    protected override bool CreateRecord => true;
    protected override bool GenerateMultipleFiles => false;
    protected override bool EnableGlobalUsings => true;

    protected override bool IsAbstractType(Type type) => base.IsAbstractType(type) || type.In(typeof(Models.IQuery), typeof(Models.ICondition));

    protected override IEnumerable<TypenameMappingBuilder> CreateAdditionalTypenameMappings()
        => new TypenameMappingBuilder[]
        {
            new TypenameMappingBuilder()
                .WithSourceType(typeof(IEvaluatable))
                .WithTargetType(typeof(IEvaluatable))
                .AddMetadata
                (
                    new MetadataBuilder().WithValue(string.Empty).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderNamespace),
                    new MetadataBuilder().WithValue(typeof(IBuilder<object>).ReplaceGenericTypeName(typeof(IEvaluatable))).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderName),
                    new MetadataBuilder().WithValue($"new {Constants.TypeNames.ComposableConditionBuilder}(source.{{property.Name}})").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderConstructorInitializeExpression),
                    new MetadataBuilder().WithValue(new Literal($"new {Constants.TypeNames.ComposableConditionBuilder}()", null)).WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderDefaultValue),
                    new MetadataBuilder().WithValue("[Name][NullableSuffix].BuildTyped()").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderMethodParameterExpression)
                ),
                new TypenameMappingBuilder()
                    .WithSourceType(typeof(ValidGroupsAttribute))
                    .WithTargetTypeName(typeof(ValidGroupsAttribute).FullName!.Replace(CodeGenerationRootNamespace, $"{ProjectName}.Abstractions"))
        }.Concat(
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
                                new MetadataBuilder().WithValue("[Name][NullableSuffix].ToBuilder()[ForcedNullableSuffix]").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderSourceExpression),
                                new MetadataBuilder().WithValue("[Name][NullableSuffix].Build()[ForcedNullableSuffix]").WithName(ClassFramework.Pipelines.MetadataNames.CustomBuilderMethodParameterExpression),
                                new MetadataBuilder().WithName(ClassFramework.Pipelines.MetadataNames.CustomEntityInterfaceTypeName).WithValue($"{ProjectName}.Abstractions.I{x.GetEntityClassName()}")
                            )
                    })
        );
}
