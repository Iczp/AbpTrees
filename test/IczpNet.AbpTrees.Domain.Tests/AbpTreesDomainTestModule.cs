//using IczpNet.AbpTrees.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace IczpNet.AbpTrees;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
//[DependsOn(
//    typeof(AbpTreesEntityFrameworkCoreTestModule)
//    )]
public class AbpTreesDomainTestModule : AbpModule
{

}
