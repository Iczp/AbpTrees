using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using IczpNet.AbpTreesDemo.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using IczpNet.AbpTrees;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpValidationModule)
)]

[DependsOn(typeof(AbpTreesDomainSharedModule))]
public class AbpTreesDemoDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTreesDemoDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpTreesDemoResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/AbpTreesDemo");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("AbpTreesDemo", typeof(AbpTreesDemoResource));
        });
    }
}
