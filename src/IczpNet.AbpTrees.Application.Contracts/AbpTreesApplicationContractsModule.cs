using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace IczpNet.AbpTrees;

[DependsOn(
    typeof(AbpTreesDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpTreesApplicationContractsModule : AbpModule
{

}
