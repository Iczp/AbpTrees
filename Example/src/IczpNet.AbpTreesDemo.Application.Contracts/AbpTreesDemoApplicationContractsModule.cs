using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpTreesDemoDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpTreesDemoApplicationContractsModule : AbpModule
{

}
