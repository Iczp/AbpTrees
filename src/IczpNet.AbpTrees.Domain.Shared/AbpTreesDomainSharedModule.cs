using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace IczpNet.AbpTrees;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class AbpTreesDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }
}
