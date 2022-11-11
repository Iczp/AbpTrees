using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace IczpNet.AbpTreesDemo.EntityFrameworkCore;

[ConnectionStringName(AbpTreesDemoDbProperties.ConnectionStringName)]
public interface IAbpTreesDemoDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
