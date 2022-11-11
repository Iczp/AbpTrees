using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace IczpNet.AbpTreesDemo.EntityFrameworkCore;

public class AbpTreesDemoHttpApiHostMigrationsDbContext : AbpDbContext<AbpTreesDemoHttpApiHostMigrationsDbContext>
{
    public AbpTreesDemoHttpApiHostMigrationsDbContext(DbContextOptions<AbpTreesDemoHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureAbpTreesDemo();
    }
}
