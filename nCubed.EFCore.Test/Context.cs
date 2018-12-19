using nCubed.EFCore.Test.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using nCubed.EFCore.Test.Fakes;

namespace nCubed.EFCore.Test
{
    public class Context
    {
        protected DbContextOptions<ProjectsContext> dbContextOptions = null;

        public Context()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            dbContextOptions = new DbContextOptionsBuilder<ProjectsContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ProjectsContext(dbContextOptions))
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
