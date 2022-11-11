using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IczpNet.AbpTreesDemo.EntityFrameworkCore;

public class AbpTreesDemoHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<AbpTreesDemoHttpApiHostMigrationsDbContext>
{
    public AbpTreesDemoHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<AbpTreesDemoHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("AbpTreesDemo"));

        return new AbpTreesDemoHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
