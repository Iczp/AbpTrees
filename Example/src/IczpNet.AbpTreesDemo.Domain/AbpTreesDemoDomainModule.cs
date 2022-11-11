using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpTreesDemoDomainSharedModule)
)]
public class AbpTreesDemoDomainModule : AbpModule
{

}
