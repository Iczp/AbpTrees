using Localization.Resources.AbpUi;
using IczpNet.AbpTreesDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpTreesDemoApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpTreesDemoHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpTreesDemoHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpTreesDemoResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
