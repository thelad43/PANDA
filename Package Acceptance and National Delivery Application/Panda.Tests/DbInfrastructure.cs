namespace Panda.Tests
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DbInfrastructure
    {
        public static PandaDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<PandaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new PandaDbContext(dbOptions);
        }
    }
}