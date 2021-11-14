using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Abstractions.Extensions;
using CrossCutting.Data.Core;
using QueryFramework.Abstractions;
using QueryFramework.Core;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository : ITestRepository
    {
        public TestEntity Add(TestEntity instance)
            => _commandProcessor.InvokeCommand(instance).HandleResult("TestEntity has not been added");

        public TestEntity Update(TestEntity instance)
            => _commandProcessor.InvokeCommand(instance).HandleResult("TestEntity has not been updated");

        public TestEntity Delete(TestEntity instance)
            => _commandProcessor.InvokeCommand(instance).HandleResult("TestEntity has not been deleted");

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

        public TestRepository(IDatabaseCommandProcessor<TestEntity> commandProcessor,
                              IDatabaseEntityRetriever<TestEntity> retriever,
                              IQueryProcessor<ITestQuery, TestEntity> queryProcessor)
        {
            _commandProcessor = commandProcessor;
            _retriever = retriever;
            _queryProcessor = queryProcessor;
        }

        private readonly IDatabaseCommandProcessor<TestEntity> _commandProcessor;
        private readonly IDatabaseEntityRetriever<TestEntity> _retriever;
        private readonly IQueryProcessor<ITestQuery, TestEntity> _queryProcessor;
    }
}
