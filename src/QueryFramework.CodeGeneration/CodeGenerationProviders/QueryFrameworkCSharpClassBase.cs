namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

public abstract partial class QueryFrameworkCSharpClassBase : CSharpClassBase
{
    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(ValueCollection<>);
    protected override string SetMethodNameFormatString => string.Empty;

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
            if (typeName.StartsWith("ExpressionFramework.Abstractions.DomainModel.I", StringComparison.InvariantCulture))
            {
                property.ConvertSinglePropertyToBuilderOnBuilder
                (
                    typeName.Replace("ExpressionFramework.Abstractions.DomainModel.", "ExpressionFramework.Abstractions.DomainModel.Builders.") + "Builder",
                    GetCustomBuilderConstructorInitializeExpression(property, typeName)
                );

                property.SetDefaultValueForBuilderClassConstructor(new Literal(GetBuilderInitialization(typeName)));
            }
        }
    }

    private static string GetBuilderInitialization(string typeName)
    {
        if (typeName == "ExpressionFramework.Abstractions.DomainModel.IExpression")
        {
            return "new ExpressionFramework.Core.DomainModel.Builders.EmptyExpressionBuilder()";
        }
        return "new " + typeName.Replace("ExpressionFramework.Abstractions.DomainModel.I", "ExpressionFramework.Core.DomainModel.Builders.") + "Builder()";
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
                .AddLiteralCodeStatements("instance.Field = new FieldExpressionBuilder().WithFieldName(fieldName);",
                                          "return instance;");
        }
    }

    private static string GetCustomBuilderConstructorInitializeExpression(ClassPropertyBuilder property, string typeName)
    {
        if (typeName == "ExpressionFramework.Abstractions.DomainModel.IExpression")
        {
            return "{0} = source.{0}.ToBuilder()";
        }

        return property.IsNullable
            ? "{0} = source.{0} == null ? null : new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.").Replace("ExpressionFramework.Abstractions.DomainModel.I", "ExpressionFramework.Core.DomainModel.Builders.") + "Builder" + "(source.{0})"
            : "{0} = new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.").Replace("ExpressionFramework.Abstractions.DomainModel.I", "ExpressionFramework.Core.DomainModel.Builders.") + "Builder" + "(source.{0})";
    }
}
