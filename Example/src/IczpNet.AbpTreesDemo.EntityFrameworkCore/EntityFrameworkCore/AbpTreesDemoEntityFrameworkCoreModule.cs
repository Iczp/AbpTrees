using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace IczpNet.AbpTreesDemo.EntityFrameworkCore;

[DependsOn(
    typeof(AbpTreesDemoDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpTreesDemoEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AbpTreesDemoDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
        });
    }
}
