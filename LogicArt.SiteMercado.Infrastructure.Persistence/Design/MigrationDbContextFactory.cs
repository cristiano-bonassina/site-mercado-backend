using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LogicArt.SiteMercado.Infrastructure.Persistence.Design
{
    public class MigrationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlServer()
                .Options;
            var context = new ApplicationDbContext(options, null);
            return context;
        }
    }
}
