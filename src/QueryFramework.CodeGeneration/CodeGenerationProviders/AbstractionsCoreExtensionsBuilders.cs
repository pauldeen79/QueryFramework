namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsCoreExtensionsBuilders : QueryFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Abstractions}/Extensions";

    protected override string SetMethodNameFormatString => "With{0}";

    public override object CreateModel()
        => GetImmutableBuilderExtensionClasses
        (
            GetCoreModels(),
            Constants.Namespaces.Abstractions,
            Constants.Namespaces.AbstractionsExtensions,
            Constants.Namespaces.AbstractionsBuilders
        ).Select(x => new ClassBuilder(x)
            .With(x => x.Methods.ForEach(y =>
            {
                // hacking here... code generation doesn't work out of the box :(
                y.GenericTypeArgumentConstraints = y.GenericTypeArgumentConstraints.Select(z => z.Replace("where T : ", "where T : I", StringComparison.Ordinal)).ToList();
            }))
            .Build()
        );
}
