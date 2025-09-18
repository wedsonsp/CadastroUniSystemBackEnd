using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sistemaws.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SistemawsDbContext>
{
    public SistemawsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SistemawsDbContext>();
        
        // String de conexão hardcoded para tempo de design
        var connectionString = "Server=MARCCIELO\\SQLEXPRESS;Database=UniSystem;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
        
        optionsBuilder.UseSqlServer(connectionString);

        return new SistemawsDbContext(optionsBuilder.Options);
    }
}
