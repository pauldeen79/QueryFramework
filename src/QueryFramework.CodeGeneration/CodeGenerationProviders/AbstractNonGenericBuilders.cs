namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractNonGenericBuilders : QueryFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Core}/Builders";

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;
    protected override string FileNameSuffix => ".nongeneric.template.generated";

    public override object CreateModel()
        => GetImmutableNonGenericBuilderClasses(
            GetAbstractModels(),
            Constants.Namespaces.Core,
            Constants.Namespaces.CoreBuilders)
        // hacking here... code generation doesn't work out of the box :(
        .Cast<IClass>()
        .Select
        (
            x => new ClassBuilder(x)
                .With(y =>
                {
                    foreach (var ctor in y.Constructors)
                    {
                        foreach (var statement in ctor.CodeStatements.OfType<LiteralCodeStatementBuilder>())
                        {
                            // hacking here... doesn't work out of the box :(
                            statement.WithStatement(statement.Statement
                                .Replace($"new {Constants.Namespaces.AbstractionsBuilders}.I", $"new {Constants.Namespaces.CoreBuilders}."));
                        }
                    }
                    
                })
                .Build()
        ).ToArray();
}
