﻿namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class OverrideQueryBuilders : QueryFrameworkCSharpClassBase
{
    public OverrideQueryBuilders(ICsharpExpressionDumper csharpExpressionDumper, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionDumper, builderPipeline, builderExtensionPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => Constants.Paths.QueryBuilders;

    public override IEnumerable<TypeBase> Model => GetBuilders(GetOverrideModels(typeof(Models.IQuery)).Result, Constants.Namespaces.CoreBuildersQueries, Constants.Namespaces.CoreQueries).Result;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override Class? BaseClass => CreateBaseclass(typeof(Models.IQuery), Constants.Namespaces.Core).Result;
    protected override string BaseClassBuilderNamespace => Constants.Namespaces.CoreBuilders;
}
