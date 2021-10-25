﻿using System;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer
{
    public class DatabaseCommandGenerator : IDatabaseCommandGenerator
    {
        public IDatabaseCommand Generate<TQuery>(TQuery query,
                                                 IQueryProcessorSettings settings,
                                                 IQueryFieldNameProvider fieldNameProvider,
                                                 bool countOnly)
            where TQuery : ISingleEntityQuery
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (fieldNameProvider == null)
            {
                throw new ArgumentNullException(nameof(fieldNameProvider));
            }

            var fieldSelectionQuery = query as IFieldSelectionQuery;
            var groupingQuery = query as IGroupingQuery;
            return new DatabaseCommandBuilder()
                .AppendPagingOuterQuery(query, settings, fieldNameProvider, countOnly)
                .AppendSelectAndDistinctClause(fieldSelectionQuery, countOnly)
                .AppendTopClause(query, settings, countOnly)
                .AppendCountOrSelectFields(query, settings, fieldNameProvider, countOnly)
                .AppendPagingPrefix(query, settings, countOnly)
                .AppendFromClause()
                .AppendTableName(query, settings)
                .AppendWhereClause(query, settings, out int paramCounter)
                .AppendGroupByClause(groupingQuery, settings)
                .AppendHavingClause(groupingQuery, settings, ref paramCounter)
                .AppendOrderByClause(query, settings, countOnly)
                .AppendPagingSuffix(query, settings, countOnly)
                .AddQueryParameters(query)
                .Build();
        }
    }
}
