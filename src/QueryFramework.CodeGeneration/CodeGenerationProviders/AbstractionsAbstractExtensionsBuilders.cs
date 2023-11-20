namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsAbstractExtensionsBuilders : QueryFrameworkCSharpClassBase, ICodeGenerationProvider
{
    public override string Path => $"{Constants.Namespaces.Abstractions}/Extensions";

    protected override string SetMethodNameFormatString => "With{0}";

    public override object CreateModel()
        => GetImmutableBuilderExtensionClasses
        (
            GetAbstractModels(),
            Constants.Namespaces.Abstractions,
            Constants.Namespaces.AbstractionsExtensions,
            Constants.Namespaces.AbstractionsBuilders
        ).Select(x => new ClassBuilder(x)
            .With(x => x.Methods.ForEach(y =>
            {
                // hacking here... code generation doesn't work out of the box :(
                foreach (var parameter in y.Parameters.Where(z => z.TypeName.ToString().Contains($"{typeof(IReadOnlyCollection<>).WithoutGenerics()}<")))
                {
                    parameter.TypeName = parameter.TypeName.Replace($"{typeof(IReadOnlyCollection<>).WithoutGenerics()}<", $"{typeof(IEnumerable<>).WithoutGenerics()}<");
                    foreach (var literalCodeStatement in y.CodeStatements.OfType<LiteralCodeStatementBuilder>().Where(z => z.Statement.ToString().Contains($"instance.{parameter.Name} = ", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        literalCodeStatement.WithStatement($"instance.{parameter.Name.ToString().Substring(0, 1).ToUpperInvariant()}{parameter.Name.ToString().Substring(1)} = new {typeof(List<>).WithoutGenerics()}<{parameter.TypeName.ToString().GetGenericArguments()}>({parameter.Name});");
                    }
                }
            }))
            .Build()
        ).ToArray();
}
