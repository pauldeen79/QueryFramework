﻿namespace QueryFramework.CodeGeneration2.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractBuildersInterfaces : QueryFrameworkCSharpClassBase
{
    public AbstractionsAbstractBuildersInterfaces(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderExtensionContext> builderExtensionPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, builderExtensionPipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => $"{Constants.Namespaces.Abstractions}/Builders";

    public override IEnumerable<TypeBase> Model => GetBuilderInterfaces(GetAbstractionsInterfaces(), Constants.Namespaces.AbstractionsBuilders, Constants.Namespaces.Abstractions, Constants.Namespaces.AbstractionsBuilders)
        .Select(x => x.ToBuilder()
            //.AddMethods(new MethodBuilder().WithName("Build").WithReturnTypeName(x.Name))
            .AddInterfaces($"IBuilder<{x.Name.ReplaceSuffix("Builder", string.Empty, StringComparison.Ordinal)}>").Build());

    protected override bool EnableEntityInheritance => true;
}
