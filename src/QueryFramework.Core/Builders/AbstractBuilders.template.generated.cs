﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 9.0.1
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace QueryFramework.Core.Builders
{
    public abstract partial class QueryBuilder<TBuilder, TEntity> : QueryBuilder, QueryFramework.Abstractions.Builders.IQueryBuilder
        where TEntity : QueryFramework.Core.Query
        where TBuilder : QueryBuilder<TBuilder, TEntity>
    {
        protected QueryBuilder(QueryFramework.Abstractions.IQuery source) : base(source)
        {
        }

        protected QueryBuilder() : base()
        {
        }

        public override QueryFramework.Abstractions.IQuery Build()
        {
            return BuildTyped();
        }

        public abstract TEntity BuildTyped();
    }
}
#nullable disable
