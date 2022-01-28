using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using ModelFramework.CodeGeneration.CodeGenerationProviders;
using ModelFramework.Common;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;

namespace QueryFramework.CodeGeneration.CodeGenerationProviders
{
    public abstract class QueryFrameworkCSharpClassBase : CSharpClassBase
    {
        protected override bool CreateCodeGenerationHeader => true;
        protected override bool EnableNullableContext => true;
        protected override Type RecordCollectionType => typeof(ValueCollection<>);

        protected override IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
            => Enumerable.Empty<ClassMethodBuilder>();

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
                if (typeName.StartsWith("QueryFramework.Abstractions.I", StringComparison.InvariantCulture))
                {
                    property.ConvertSinglePropertyToBuilderOnBuilder
                    (
                        //typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.", StringComparison.InvariantCulture) + "Builder",
                        typeName.Replace("QueryFramework.Abstractions.", "QueryFramework.Abstractions.Builders.") + "Builder",
                        //TODO: Look if we can fix this in ModelFramework
                        property.IsNullable
                            ? "{0} = source.{0} == null ? null : new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.") + "Builder" + "(source.{0});"
                            : "{0} = new " + typeName.Replace("QueryFramework.Core.", "QueryFramework.Core.Builders.") + "Builder" + "(source.{0});"
                    );

                    //TODO: Look if we can fix this in ModelFramework
                    property.SetDefaultValueForBuilderClassConstructor(new Literal("new " + typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.") + "Builder()"));
                    if (property.IsNullable)
                    {
                        //TODO: Fix in ModelFramework
                        property.ReplaceMetadata(ModelFramework.Objects.MetadataNames.CustomBuilderMethodParameterExpression, "{0}?.Build()");
                    }
                }
                else if (typeName.Contains("Collection<QueryFramework."))
                {
                    property.ConvertCollectionPropertyToBuilderOnBuilder
                    (
                        false,
                        typeof(ValueCollection<>).WithoutGenerics(),
                        //typeName.Replace("QueryFramework.Abstractions.I", "QueryFramework.Core.Builders.", StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                        typeName.Replace("QueryFramework.Abstractions.", "QueryFramework.Abstractions.Builders.").ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                    );
                }
                else if (typeName.Contains("Collection<System.String"))
                {
                    property.AddMetadata(ModelFramework.Objects.MetadataNames.CustomBuilderMethodParameterExpression, $"new {typeof(ValueCollection<string>).FullName?.FixTypeName()}({{0}})");
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

        //TODO: Move to ModelFramework (CSharpClassBase)
        protected IClass[] GetImmutableBuilderClasses(ClassBuilder[] models, string entitiesNamespace, string buildersNamespace)
            => models.Select
            (
                x => CreateBuilder(new ClassBuilder(x.Build())
                                    .WithName(x.Name.Substring(1))
                                    .WithNamespace(entitiesNamespace)
                                    .Chain(y => FixImmutableBuilderProperties(y))
                                    .Build()
                                    .ToImmutableClass(CreateMyImmutableClassSettings()), buildersNamespace
                                  )
                                  .AddInterfaces($"QueryFramework.Abstractions.Builders.{x.Name}Builder")
                                  .Build()
            ).ToArray();

        //TODO: Move to ModelFramework (CSharpClassBase)
        protected IClass[] GetImmutableClasses(ClassBuilder[] models, string entitiesNamespace)
            => models.Select
            (
                x => new ClassBuilder(x.Build())
                      .WithName(x.Name.Substring(1))
                      .WithNamespace(entitiesNamespace)
                      .Chain(y => FixImmutableBuilderProperties(y))
                      .Build()
                      .ToImmutableClassBuilder(CreateMyImmutableClassSettings())
                      .WithRecord()
                      .WithPartial()
                      .AddInterfaces($"{x.Namespace}.{x.Name}")
                      .Build()
            ).ToArray();

        //TODO: Remove after move to code above to ModelFramework, or making the method protected there
        private ImmutableClassSettings CreateMyImmutableClassSettings()
            => new ImmutableClassSettings(newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
                                          validateArgumentsInConstructor: true);

        protected static ClassBuilder[] GetModels()
            => new[]
            {
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"OpenBracket",
                            TypeName = @"System.Boolean",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"CloseBracket",
                            TypeName = @"System.Boolean",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Field",
                            TypeName = @"QueryFramework.Abstractions.IQueryExpression",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Operator",
                            TypeName = @"QueryFramework.Abstractions.QueryOperator",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Value",
                            TypeName = @"System.Object",
                            IsNullable = true,
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Combination",
                            TypeName = @"QueryFramework.Abstractions.QueryCombination",
                        },
                    } ),
                    Name = @"IQueryCondition",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"FieldName",
                            TypeName = @"System.String",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Function",
                            TypeName = @"QueryFramework.Abstractions.IQueryExpressionFunction",
                            IsNullable = true,
                        },
                    } ),
                    Name = @"IQueryExpression",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"InnerFunction",
                            TypeName = @"QueryFramework.Abstractions.IQueryExpressionFunction",
                            IsNullable = true,
                        },
                    } ),
                    Name = @"IQueryExpressionFunction",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Name",
                            TypeName = @"System.String",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Value",
                            TypeName = @"System.Object",
                        },
                    } ),
                    Name = @"IQueryParameter",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Name",
                            TypeName = @"System.String",
                        },
                    } ),
                    Name = @"IQueryParameterValue",
                },
                new ModelFramework.Objects.Builders.ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>(new[]
                    {
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Field",
                            TypeName = @"QueryFramework.Abstractions.IQueryExpression",
                        },
                        new ModelFramework.Objects.Builders.ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Order",
                            TypeName = @"QueryFramework.Abstractions.QuerySortOrderDirection",
                        },
                    } ),
                    Name = @"IQuerySortOrder",
                },
            };
    }
}
