using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace IczpNet.AbpTrees;

[DependsOn(
    typeof(AbpTreesDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpTreesApplicationContractsModule : AbpModule
{

}
