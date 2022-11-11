using IczpNet.AbpTreesDemo.Localization;
using Volo.Abp.Application.Services;

namespace IczpNet.AbpTreesDemo;

public abstract class AbpTreesDemoAppService : ApplicationService
{
    protected AbpTreesDemoAppService()
    {
        LocalizationResource = typeof(AbpTreesDemoResource);
        ObjectMapperContext = typeof(AbpTreesDemoApplicationModule);
    }
}
