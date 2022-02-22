﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.1
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryFramework.CodeGeneration.CodeGenerationProviders
{
#nullable enable
    public partial class QueryFrameworkCSharpClassBase
    {
        protected static ITypeBase[] GetModels()
        {
            return new[]
            {
                new ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new List<ClassPropertyBuilder>(new[]
                    {
                        new ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Name",
                            TypeName = @"System.String",
                        },
                        new ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Value",
                            TypeName = @"System.Object",
                        },
                    } ),
                    Name = @"IQueryParameter",
                },
                new ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new List<ClassPropertyBuilder>(new[]
                    {
                        new ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Name",
                            TypeName = @"System.String",
                        },
                    } ),
                    Name = @"IQueryParameterValue",
                },
                new ClassBuilder
                {
                    Namespace = @"QueryFramework.Abstractions",
                    Properties = new List<ClassPropertyBuilder>(new[]
                    {
                        new ClassPropertyBuilder
                        {
                            HasSetter = false,
                            Name = @"Field",
                            TypeName = @"ExpressionFramework.Abstractions.DomainModel.IExpression",
                        },
                        new ClassPropertyBuilder
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
    }
#nullable restore
}
