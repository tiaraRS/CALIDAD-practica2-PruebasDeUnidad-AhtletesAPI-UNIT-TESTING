using AthletesRestAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.AthleteRepositoryUT
{
    public class BaseTest
    {
        protected AthleteDBContext ctx;
        public BaseTest(AthleteDBContext ctx = null)
        {
            this.ctx = ctx ?? GetInMemoryDBContext();
        }
        protected AthleteDBContext GetInMemoryDBContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<AthleteDBContext>();
            var options = builder.UseInMemoryDatabase("testDB").UseInternalServiceProvider(serviceProvider).Options;

            AthleteDBContext dbContext = new AthleteDBContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }


    }
}
