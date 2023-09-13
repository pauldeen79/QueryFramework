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
                foreach (var method in y.Methods.Where(z => z.Name.ToString().StartsWith("With") && z.Parameters.Count == 2 && z.Parameters[1].TypeName.ToString().Contains("System.Collections.Generic.")))
                {
                    method.CodeStatements.OfType<LiteralCodeStatementBuilder>().First().Statement.Replace(";", ".ToList();");
                }

                y.Methods.RemoveAll(z => typeof(IQuery).GetProperties().Select(p => $"With{p.Name}").Contains(z.Name.ToString()));
            })
            .Build()
        ).ToArray();
}
