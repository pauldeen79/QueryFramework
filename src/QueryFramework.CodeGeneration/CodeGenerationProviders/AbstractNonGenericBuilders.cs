﻿namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractNonGenericBuilders : QueryFrameworkCSharpClassBase
{
    public AbstractNonGenericBuilders(IMediator mediator, ICsharpExpressionDumper csharpExpressionDumper) : base(mediator, csharpExpressionDumper)
    {
    }

    public override async Task<IEnumerable<TypeBase>> GetModel()
        => await GetNonGenericBuilders(await GetAbstractModels(), Constants.Namespaces.CoreBuilders, Constants.Namespaces.Core);

    public override string Path => $"{Constants.Namespaces.Core}/Builders";

    protected override bool AddNullChecks => false; // not needed for abstract builders, because each derived class will do its own validation
    protected override bool AddBackingFields => true; // backing fields are added when using null checks... so we need to add this explicitly

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;

    protected override string FilenameSuffix => ".nongeneric.template.generated";

    // Do not generate 'With' methods. Do this on the interfaces instead.
    protected override string SetMethodNameFormatString => string.Empty;
    protected override string AddMethodNameFormatString => string.Empty;
}
