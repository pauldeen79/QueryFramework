namespace QueryFramework.CodeGeneration2.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractExtensionsBuilders : QueryFrameworkCSharpClassBase
{
    public AbstractionsAbstractExtensionsBuilders(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, builderExtensionPipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => $"{Constants.Namespaces.Abstractions}/Extensions";

    public override IEnumerable<TypeBase> Model => GetBuilderExtensions(GetAbstractionsInterfaces(), Constants.Namespaces.AbstractionsBuilders, Constants.Namespaces.Abstractions, Constants.Namespaces.AbstractionsBuildersExtensions);

    protected override bool EnableEntityInheritance => true;
}
