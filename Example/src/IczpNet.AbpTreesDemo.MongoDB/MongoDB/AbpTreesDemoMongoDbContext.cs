using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace IczpNet.AbpTreesDemo.MongoDB;

[ConnectionStringName(AbpTreesDemoDbProperties.ConnectionStringName)]
public class AbpTreesDemoMongoDbContext : AbpMongoDbContext, IAbpTreesDemoMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureAbpTreesDemo();
    }
}
