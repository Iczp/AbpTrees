using IczpNet.AbpTreesDemo.Departments;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace IczpNet.AbpTreesDemo.EntityFrameworkCore;

[ConnectionStringName(AbpTreesDemoDbProperties.ConnectionStringName)]
public class AbpTreesDemoDbContext : AbpDbContext<AbpTreesDemoDbContext>, IAbpTreesDemoDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public AbpTreesDemoDbContext(DbContextOptions<AbpTreesDemoDbContext> options)
        : base(options)
    {

    }
    /// <summary>
    /// Department
    /// </summary>
    public DbSet<Department> Department { get; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAbpTreesDemo();
    }
}
