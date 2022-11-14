using IczpNet.AbpTreesDemo.Departments;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace IczpNet.AbpTreesDemo.EntityFrameworkCore;

[ConnectionStringName(AbpTreesDemoDbProperties.ConnectionStringName)]
public interface IAbpTreesDemoDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    /// <summary>
    /// Department
    /// </summary>
    DbSet<Department> Department { get; }
}
