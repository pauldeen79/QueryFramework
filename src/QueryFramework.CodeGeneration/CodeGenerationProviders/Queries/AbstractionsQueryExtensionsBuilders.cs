namespace QueryFramework.CodeGeneration.CodeGenerationProviders.Queries;

[ExcludeFromCodeCoverage]
public class AbstractionsQueryExtensionsBuilders : QueryFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Abstractions}/Extensions";

    protected override string SetMethodNameFormatString => "With{0}";

    public override object CreateModel()
        => GetImmutableBuilderExtensionClasses
        (
            GetOverrideModels(typeof(IQuery)),
            Constants.Namespaces.AbstractionsQueries,
            Constants.Namespaces.AbstractionsExtensions,
            Constants.Namespaces.AbstractionsBuildersQueries
        ).Select(x => new ClassBuilder(x)
            .Chain(y =>
            {
                // hacking here... code generation doesn't work out of the box :(
                foreach (var method in y.Methods.Where(z => z.Name.ToString().StartsWith("With") && z.Parameters.Count == 2 && z.Parameters[1].TypeName.ToString().Contains("System.Collections.Generic.")))
                {
                    method.Parameters.ForEach(parameter => parameter.TypeName = parameter.TypeName.Replace($"{typeof(IReadOnlyCollection<>).WithoutGenerics()}<", $"{typeof(IEnumerable<>).WithoutGenerics()}<"));
                    var x = method.CodeStatements.OfType<LiteralCodeStatementBuilder>().First();
                    x.WithStatement(x.Statement.Replace(";", ".ToList();"));
                }

                y.Methods.RemoveAll(z => typeof(IQuery).GetProperties().Select(p => $"With{p.Name}").Contains(z.Name.ToString()));
                y.Methods.ForEach(z => z.GenericTypeArgumentConstraints = z.GenericTypeArgumentConstraints.Select(a => a.Replace("where T : ", "where T : I", StringComparison.Ordinal)).ToList());
            })
            .Build()
        ).ToArray();
}
