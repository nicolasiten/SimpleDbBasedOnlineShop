using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Data;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public abstract class DbContextTestBase
    {
        protected readonly P3Referential DbContextRealDb;
        protected readonly DbContextOptions<P3Referential> DbContextOptionsRealDb;

        protected readonly P3Referential DbContextInMemoryDb;
        protected readonly DbContextOptions<P3Referential> DbContextOptionsInMemory;

        protected DbContextTestBase()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            DbContextOptionsRealDb = new DbContextOptionsBuilder<P3Referential>().UseSqlServer(config.GetConnectionString("P3Referential")).Options;
            DbContextRealDb = new P3Referential(DbContextOptionsRealDb);

            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            DbContextOptionsInMemory = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;
            DbContextInMemoryDb = new P3Referential(DbContextOptionsInMemory);
        }
    }
}
