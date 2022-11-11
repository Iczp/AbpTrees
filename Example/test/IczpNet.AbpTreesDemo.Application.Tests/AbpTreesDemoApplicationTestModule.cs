using Volo.Abp.Modularity;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpTreesDemoApplicationModule),
    typeof(AbpTreesDemoDomainTestModule)
    )]
public class AbpTreesDemoApplicationTestModule : AbpModule
{

}
