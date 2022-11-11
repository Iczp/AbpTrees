using IczpNet.AbpTrees;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace IczpNet.AbpTrees;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpTreesDomainSharedModule)
)]
public class AbpTreesDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpTreesDomainModule>();
        });

        context.Services.AddTransient(typeof(ITreeManager<>), typeof(TreeManager<>));
        context.Services.AddTransient(typeof(ITreeManager<,>), typeof(TreeManager<,>));
        context.Services.AddTransient(typeof(ITreeManager<,,>), typeof(TreeManager<,,>));
    }
}
