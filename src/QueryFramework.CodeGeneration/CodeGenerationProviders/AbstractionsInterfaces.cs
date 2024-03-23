namespace QueryFramework.CodeGeneration2.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces : QueryFrameworkCSharpClassBase
{
    public AbstractionsInterfaces(ICsharpExpressionDumper csharpExpressionDumper, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionDumper, builderPipeline, builderExtensionPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => Constants.Namespaces.Abstractions;

    public override IEnumerable<TypeBase> Model => GetEntityInterfaces(GetAbstractionsInterfaces(), Constants.Namespaces.Core, Constants.Namespaces.Abstractions);

    protected override bool EnableEntityInheritance => true;
}
