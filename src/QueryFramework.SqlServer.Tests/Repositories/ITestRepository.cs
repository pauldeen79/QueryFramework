﻿using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using QueryFramework.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    public interface ITestRepository : IQueryProcessor<ITestQuery, TestEntity>
    {
        TestEntity Add(TestEntity instance);

        TestEntity Update(TestEntity instance);

        TestEntity Delete(TestEntity instance);

        TestEntity? FindOne(IDatabaseCommand command);

        IReadOnlyCollection<TestEntity> FindMany(IDatabaseCommand command);

        TestEntity? Find(TestEntityIdentity identity);
    }
}