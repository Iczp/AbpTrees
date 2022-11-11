using Volo.Abp.Modularity;

namespace IczpNet.AbpTrees;

[DependsOn(
    typeof(AbpTreesApplicationModule),
    typeof(AbpTreesDomainTestModule)
    )]
public class AbpTreesApplicationTestModule : AbpModule
{

}
