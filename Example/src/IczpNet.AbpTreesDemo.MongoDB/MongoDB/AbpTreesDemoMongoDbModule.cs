using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace IczpNet.AbpTreesDemo.MongoDB;

[DependsOn(
    typeof(AbpTreesDemoDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class AbpTreesDemoMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<AbpTreesDemoMongoDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
