using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

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

        //context.Services.AddTransient(typeof(ITreeManager<>), typeof(TreeManager<>));
        context.Services.AddTransient(typeof(ITreeManager<,>), typeof(TreeManager<,>));
        context.Services.AddTransient(typeof(ITreeManager<,,>), typeof(TreeManager<,,>));
        context.Services.AddTransient(typeof(ITreeManager<,,,>), typeof(TreeManager<,,,>));
        context.Services.AddTransient(typeof(ITreeManager<,,,,>), typeof(TreeManager<,,,,>));
    }
}
