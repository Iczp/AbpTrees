using IczpNet.AbpTreesDemo.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace IczpNet.AbpTreesDemo;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(AbpTreesDemoEntityFrameworkCoreTestModule)
    )]
public class AbpTreesDemoDomainTestModule : AbpModule
{

}
