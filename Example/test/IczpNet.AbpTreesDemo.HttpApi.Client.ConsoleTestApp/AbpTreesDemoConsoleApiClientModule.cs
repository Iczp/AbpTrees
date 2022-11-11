using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace IczpNet.AbpTreesDemo;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTreesDemoHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class AbpTreesDemoConsoleApiClientModule : AbpModule
{

}
