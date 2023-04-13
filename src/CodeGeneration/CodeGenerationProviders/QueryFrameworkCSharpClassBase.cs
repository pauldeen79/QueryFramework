namespace CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract partial class QueryFrameworkCSharpClassBase : CSharpClassBase
{
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override string SetMethodNameFormatString => string.Empty;
    protected override string FileNameSuffix => ".template.generated";
    protected override bool UseLazyInitialization => false; // this needs to be disabled, because extension method-based builders currently don't support this

    protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
    {
        if (instance.Namespace == "QueryFramework.Core")
        {
            return forCreate
                ? "QueryFramework.Core." + instance.Name
                : "QueryFramework.Abstractions.I" + instance.Name;
        }

        return string.Empty;
    }

    protected override void FixImmutableBuilderProperties(ClassBuilder classBuilder)
    {
        foreach (var property in classBuilder.Properties)
        {
            var typeName = property.TypeName.FixTypeName();
            if (typeName.StartsWith("ExpressionFramework.Domain.", StringComparison.InvariantCulture))
            {
                property.ConvertSinglePropertyToBuilderOnBuilder
                (
                    typeName.Replace("ExpressionFramework.Domain.", "ExpressionFramework.Domain.Builders.") + "Builder",
                    GetCustomBuilderConstructorInitializeExpression(property, typeName)
                );

                property.SetDefaultValueForBuilderClassConstructor(new Literal(GetBuilderInitialization(typeName)));
            }
        }
    }

    private static string GetBuilderInitialization(string typeName)
    {
        if (typeName == "ExpressionFramework.Domain.Expression")
        {
            return "new ExpressionFramework.Domain.Builders.Expressions.EmptyExpressionBuilder()";
        }
        return "new " + typeName.Replace("ExpressionFramework.Domain.", "ExpressionFramework.Domain.Builders.") + "Builder()";
    }

    protected override IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
    {
        if (c.Properties.Any(p => p.Name == "Field"))
        {
            yield return new ClassMethodBuilder()
                .WithName("WithField")
                .WithStatic()
                .WithExtensionMethod()
                .WithTypeName($"{c.Name}Builder")
                .AddParameter("instance", $"{c.Name}Builder")
                .AddParameter("fieldName", typeof(string))
                .AddLiteralCodeStatements("instance.Field = new FieldExpressionBuilder().WithExpression(new ContextExpressionBuilder()).WithFieldNameExpression(new ConstantExpressionBuilder().WithValue(fieldName));",
                                          "return instance;");
        }
    }

    private static string GetCustomBuilderConstructorInitializeExpression(ClassPropertyBuilder property, string typeName)
    {
        if (typeName == "ExpressionFramework.Domain.Expression")
        {
            return "{0} = ExpressionBuilderFactory.Create(source.{0})";
        }

        return property.IsNullable
            ? "{0} = source.{0} == null ? null : new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.").Replace("ExpressionFramework.Domain.", "ExpressionFramework.Domain.Builders.") + "Builder" + "(source.{0})"
            : "{0} = new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.").Replace("ExpressionFramework.Domain.", "ExpressionFramework.Domain.Builders.") + "Builder" + "(source.{0})";
    }
}
