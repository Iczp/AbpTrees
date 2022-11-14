using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using IczpNet.AbpTrees;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpTreesDemoDomainModule),
    typeof(AbpTreesDemoApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
[DependsOn(typeof(AbpTreesApplicationModule))]
public class AbpTreesDemoApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpTreesDemoApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpTreesDemoApplicationModule>(validate: true);
        });
    }
}
