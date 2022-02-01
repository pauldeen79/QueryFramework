namespace QueryFramework.CodeGeneration.CodeGenerationProviders;

public abstract partial class QueryFrameworkCSharpClassBase
{
    protected static ITypeBase[] GetModels()
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
        }.Select(x => x.Build()).ToArray();
}
