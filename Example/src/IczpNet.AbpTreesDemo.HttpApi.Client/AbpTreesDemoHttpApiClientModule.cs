using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpTreesDemoApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpTreesDemoHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(AbpTreesDemoApplicationContractsModule).Assembly,
            AbpTreesDemoRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTreesDemoHttpApiClientModule>();
        });

    }
}
