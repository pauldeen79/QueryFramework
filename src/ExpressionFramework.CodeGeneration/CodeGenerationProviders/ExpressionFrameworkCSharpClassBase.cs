namespace ExpressionFramework.CodeGeneration.CodeGenerationProviders;

public abstract partial class ExpressionFrameworkCSharpClassBase : CSharpClassBase
{
    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(ValueCollection<>);
    protected override string SetMethodNameFormatString => string.Empty;

    protected override string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
    {
        if (instance.Namespace == "ExpressionFramework.Core.DomainModel")
        {
            return forCreate
                ? "ExpressionFramework.Core.DomainModel." + instance.Name
                : "ExpressionFramework.Abstractions.DomainModel.I" + instance.Name;
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

                property.SetDefaultValueForBuilderClassConstructor(new Literal("new " + typeName.Replace("ExpressionFramework.Abstractions.DomainModel.I", "ExpressionFramework.Core.DomainModel.Builders.") + "Builder()"));
            }
            else if (typeName.Contains("Collection<ExpressionFramework."))
            {
                property.ConvertCollectionPropertyToBuilderOnBuilder
                (
                    false,
                    typeof(ValueCollection<>).WithoutGenerics(),
                    typeName.Replace("ExpressionFramework.Abstractions.DomainModel.", "ExpressionFramework.Core.DomainModel.Builders.").ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                );
            }
            else if (typeName.Contains("Collection<System.String"))
            {
                property.AddMetadata(ModelFramework.Objects.MetadataNames.CustomBuilderMethodParameterExpression, $"new {typeof(ValueCollection<string>).FullName.FixTypeName()}({{0}})");
            }
            else if (typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
            {
                property.SetDefaultArgumentValueForWithMethod(true);
                if (property.Name == nameof(ClassProperty.HasGetter) || property.Name == nameof(ClassProperty.HasSetter))
                {
                    property.SetDefaultValueForBuilderClassConstructor(new Literal("true"));
                }
            }
        }
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
                .AddLiteralCodeStatements("instance.Field.FieldName = fieldName;",
                                          "return instance;");
        }
    }

    private static string GetCustomBuilderConstructorInitializeExpression(ClassPropertyBuilder property, string typeName)
    {
        if (typeName == "ExpressionFramework.Abstractions.DomainModel.IExpressionFunction")
        {
            return property.IsNullable
                ? "{0} = source.{0} == null ? null : source.{0}.ToBuilder()"
                : "{0} = source.{0}.ToBuilder()";
        }

        return property.IsNullable
            ? "{0} = source.{0} == null ? null : new " + typeName.Replace("ExpressionFramework.Abstractions.DomainModel.I", "ExpressionFramework.Core.DomainModel.Builders.") + "Builder" + "(source.{0})"
            : "{0} = new " + typeName.Replace("ExpressionFramework.Abstractions.DomainModel.I", "ExpressionFramework.Core.DomainModel.Builders.") + "Builder" + "(source.{0})";
    }

    protected static Type[] GetModels()
        => new[]
        {
            typeof(ICondition),
            typeof(IExpression),
            typeof(IExpressionFunction),
        };
}
