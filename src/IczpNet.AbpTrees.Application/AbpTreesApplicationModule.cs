using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace IczpNet.AbpTrees;

[DependsOn(
    typeof(AbpTreesDomainModule),
    typeof(AbpTreesApplicationContractsModule),
    typeof(AbpDddApplicationModule)
)]
public class AbpTreesApplicationModule : AbpModule
{
}
