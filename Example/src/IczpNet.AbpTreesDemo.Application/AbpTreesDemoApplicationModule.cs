using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using IczpNet.AbpTrees;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpTreesDemoDomainModule),
    typeof(AbpTreesDemoApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule)
    )]
[DependsOn(typeof(AbpTreesApplicationModule))]
public class AbpTreesDemoApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpTreesDemoApplicationModule>();
    }
}
