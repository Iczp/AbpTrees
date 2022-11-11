using IczpNet.AbpTreesDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace IczpNet.AbpTreesDemo;

public abstract class AbpTreesDemoController : AbpControllerBase
{
    protected AbpTreesDemoController()
    {
        LocalizationResource = typeof(AbpTreesDemoResource);
    }
}
