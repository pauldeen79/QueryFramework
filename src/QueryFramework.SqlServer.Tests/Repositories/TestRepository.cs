using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using QueryFramework.Abstractions;
using QueryFramework.Core;

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

        public TestEntity? Find(TestEntityIdentity identity)
            => _queryProcessor.FindOne(new TestQuery(new[] { new QueryCondition("Id", QueryOperator.Equal, identity.Id) }));

        public IPagedResult<TestEntity> FindPaged(ITestQuery query)
            => _queryProcessor.FindPaged(query);

        // Added as an example to work with Sql directly from the repository, in case query framework does not support the sql constructs you need
        public TestEntity? FindUsingCommand(TestEntityIdentity identity)
            => _retriever.FindOne(new SqlTextCommand("SELECT * FROM [Table] WHERE [Id] = @Id", new { Id = identity.Id } ));

        public TestRepository(IAddDatabaseCommandProcessor<TestEntity> addProcessor,
                              IUpdateDatabaseCommandProcessor<TestEntity> updateProcessor,
                              IDeleteDatabaseCommandProcessor<TestEntity> deleteProcessor,
                              IDatabaseEntityRetriever<TestEntity> retriever,
                              IQueryProcessor<ITestQuery, TestEntity> queryProcessor)
        {
            _addProcessor = addProcessor;
            _updateProcessor = updateProcessor;
            _deleteProcessor = deleteProcessor;
            _retriever = retriever;
            _queryProcessor = queryProcessor;
        }

        private readonly IAddDatabaseCommandProcessor<TestEntity> _addProcessor;
        private readonly IUpdateDatabaseCommandProcessor<TestEntity> _updateProcessor;
        private readonly IDeleteDatabaseCommandProcessor<TestEntity> _deleteProcessor;
        private readonly IDatabaseEntityRetriever<TestEntity> _retriever;
        private readonly IQueryProcessor<ITestQuery, TestEntity> _queryProcessor;
    }
}
