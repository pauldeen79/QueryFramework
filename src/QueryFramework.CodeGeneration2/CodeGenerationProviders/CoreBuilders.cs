﻿namespace QueryFramework.CodeGeneration2.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders : QueryFrameworkCSharpClassBase
{
    public CoreBuilders(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, builderExtensionPipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
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
