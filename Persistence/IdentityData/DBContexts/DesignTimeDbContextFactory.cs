using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.IdentityData.DBContexts;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RestaurantIdentityDBContexts>
{
    public RestaurantIdentityDBContexts CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
           ;
        var config = builder.Build();
        var conn = config.GetConnectionString("IdentityConnection")
                 
                   ?? "Server=.;Database=Restaurant_DB;Trusted_Connection=True;MultipleActiveResultSets=true;trust server certificate=true";
        var options = new DbContextOptionsBuilder<RestaurantIdentityDBContexts>()
            .UseSqlServer(conn)
            .Options;
        return new RestaurantIdentityDBContexts(options);
    }
}