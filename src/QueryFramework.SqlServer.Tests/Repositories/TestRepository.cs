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
            => _addProcessor.InvokeCommand(instance);

        public TestEntity Update(TestEntity instance)
            => _updateProcessor.InvokeCommand(instance);

        public TestEntity Delete(TestEntity instance)
            => _deleteProcessor.InvokeCommand(instance);

        public TestEntity? FindOne(ITestQuery query)
            => _queryProcessor.FindOne(query);

        public IReadOnlyCollection<TestEntity> FindMany(ITestQuery query)
            => _queryProcessor.FindMany(query);

        public IPagedResult<TestEntity> FindPaged(ITestQuery query)
            => _queryProcessor.FindPaged(query);

        public TestEntity? Find(TestEntityIdentity identity)
            => _retriever.FindOne(new Mock<IDatabaseCommand>().Object);

        public TestRepository(IDatabaseCommandProcessor<TestEntity> addProcessor,
                              IDatabaseCommandProcessor<TestEntity> updateProcessor,
                              IDatabaseCommandProcessor<TestEntity> deleteProcessor,
                              IDatabaseEntityRetriever<TestEntity> retriever,
                              IQueryProcessor<ITestQuery, TestEntity> queryProcessor)
        {
            _addProcessor = addProcessor;
            _updateProcessor = updateProcessor;
            _deleteProcessor = deleteProcessor;
            _retriever = retriever;
            _queryProcessor = queryProcessor;
        }

        private readonly IDatabaseCommandProcessor<TestEntity> _addProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _updateProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _deleteProcessor;
        private readonly IDatabaseEntityRetriever<TestEntity> _retriever;
        private readonly IQueryProcessor<ITestQuery, TestEntity> _queryProcessor;
    }
}
