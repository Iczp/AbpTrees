using IczpNet.AbpTrees;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpTreesDemoDomainSharedModule)
)]
[DependsOn(typeof(AbpTreesDomainModule))]
public class AbpTreesDemoDomainModule : AbpModule
{

}
