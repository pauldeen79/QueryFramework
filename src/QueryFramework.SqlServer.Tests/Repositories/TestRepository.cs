using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using Moq;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository : ITestRepository
    {
        public TestEntity Add(TestEntity instance)
        {
            return _addProcessor.InvokeCommand(instance);
        }

        public TestEntity Update(TestEntity instance)
        {
            return _updateProcessor.InvokeCommand(instance);
        }

        public TestEntity Delete(TestEntity instance)
        {
            return _deleteProcessor.InvokeCommand(instance);
        }

        public TestEntity? FindOne(ITestQuery query)
        {
            return _queryProcessor.FindOne(query);
        }

        public IReadOnlyCollection<TestEntity> FindMany(ITestQuery query)
        {
            return _queryProcessor.FindMany(query);
        }

        public IPagedResult<TestEntity> FindPaged(ITestQuery query)
        {
            return _queryProcessor.FindPaged(query);
        }

        public TestEntity? FindOne(IDatabaseCommand command)
        {
            return _findProcessor.FindOne(command);
        }

        public IReadOnlyCollection<TestEntity> FindMany(IDatabaseCommand command)
        {
            return _findProcessor.FindMany(command);
        }

        public TestEntity? Find(TestEntityIdentity identity)
        {

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            return FindOne(new Mock<IDatabaseCommand>().Object);
        }

        public TestRepository(IDatabaseCommandProcessor<TestEntity> addProcessor,
                              IDatabaseCommandProcessor<TestEntity> updateProcessor,
                              IDatabaseCommandProcessor<TestEntity> deleteProcessor,
                              IDatabaseCommandProcessor<TestEntity> findProcessor,
                              IQueryProcessor<ITestQuery, TestEntity> queryProcessor)
        {
            _addProcessor = addProcessor;
            _updateProcessor = updateProcessor;
            _deleteProcessor = deleteProcessor;
            _findProcessor = findProcessor;
            _queryProcessor = queryProcessor;
        }

        private readonly IDatabaseCommandProcessor<TestEntity> _addProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _updateProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _deleteProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _findProcessor;
        private readonly IQueryProcessor<ITestQuery, TestEntity> _queryProcessor;
    }
}
