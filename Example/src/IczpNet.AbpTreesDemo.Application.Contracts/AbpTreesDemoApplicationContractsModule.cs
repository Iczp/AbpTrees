using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using IczpNet.AbpTrees;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpTreesDemoDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
[DependsOn(typeof(AbpTreesApplicationContractsModule))]
public class AbpTreesDemoApplicationContractsModule : AbpModule
{

}
