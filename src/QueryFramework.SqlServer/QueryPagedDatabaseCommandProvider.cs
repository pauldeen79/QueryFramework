﻿using System;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using CrossCutting.Data.Sql.Builders;
using QueryFramework.Abstractions.Queries;
using QueryFramework.SqlServer.Abstractions;
using QueryFramework.SqlServer.Extensions;

namespace QueryFramework.SqlServer
{
    public class QueryPagedDatabaseCommandProvider<TQuery> : IPagedDatabaseCommandProvider<TQuery>
        where TQuery : ISingleEntityQuery
    {
        private IQueryFieldProvider FieldProvider { get; }
        private IQueryProcessorSettings Settings { get; }

        public QueryPagedDatabaseCommandProvider(IQueryFieldProvider fieldProvider, IQueryProcessorSettings settings)
        {
            FieldProvider = fieldProvider;
            Settings = settings;
        }

        public IDatabaseCommand Create(TQuery source, DatabaseOperation operation)
            => CreatePaged(source, operation, 0, 0).DataCommand;

        public IPagedDatabaseCommand CreatePaged(TQuery source, DatabaseOperation operation, int offset, int pageSize)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only select operation is supported");
            }

            var fieldSelectionQuery = source as IFieldSelectionQuery;
            var groupingQuery = source as IGroupingQuery;
            var parameterizedQuery = source as IParameterizedQuery;
            return new PagedDatabaseCommand(CreateCommand(source, fieldSelectionQuery, groupingQuery, parameterizedQuery, false),
                                            CreateCommand(source, fieldSelectionQuery, groupingQuery, parameterizedQuery, true),
                                            offset,
                                            pageSize);
        }

        private IDatabaseCommand CreateCommand(TQuery source,
                                               IFieldSelectionQuery? fieldSelectionQuery,
                                               IGroupingQuery? groupingQuery,
                                               IParameterizedQuery? parameterizedQuery,
                                               bool countOnly)
            => new PagedSelectCommandBuilder()
                .Select(source, Settings, FieldProvider, fieldSelectionQuery)
                .Top(source, Settings)
                .Offset(source)
                .Distinct(fieldSelectionQuery)
                .From(source, Settings)
                .Where(source, Settings, FieldProvider, out int paramCounter)
                .GroupBy(groupingQuery, Settings, FieldProvider)
                .Having(groupingQuery, Settings, FieldProvider, ref paramCounter)
                .OrderBy(source, Settings, FieldProvider, countOnly)
                .WithParameters(parameterizedQuery)
                .Build(countOnly);
    }
}