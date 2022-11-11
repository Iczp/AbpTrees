using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace IczpNet.AbpTrees;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpTreesInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTreesInstallerModule>();
        });
    }
}
