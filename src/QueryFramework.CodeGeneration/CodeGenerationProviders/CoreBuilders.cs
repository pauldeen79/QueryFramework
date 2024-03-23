namespace QueryFramework.CodeGeneration2.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders : QueryFrameworkCSharpClassBase
{
    public CoreBuilders(ICsharpExpressionDumper csharpExpressionDumper, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionDumper, builderPipeline, builderExtensionPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => $"{Constants.Namespaces.Core}/Builders";

    public override IEnumerable<TypeBase> Model
        => GetBuilders
        (
            GetCoreModels(),
            Constants.Namespaces.CoreBuilders,
            Constants.Namespaces.Core
        );
}
