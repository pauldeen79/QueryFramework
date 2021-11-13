﻿using CrossCutting.Data.Abstractions;

namespace QueryFramework.SqlServer.Tests.Repositories
{
    public interface IAddDatabaseCommandProcessor<T> : IDatabaseCommandProcessor<T> where T: class
    {
    }
}
