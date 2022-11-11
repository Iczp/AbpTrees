using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpTreesDemoInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTreesDemoInstallerModule>();
        });
    }
}
