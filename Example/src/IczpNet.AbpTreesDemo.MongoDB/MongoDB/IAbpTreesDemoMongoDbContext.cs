using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace IczpNet.AbpTreesDemo.MongoDB;

[ConnectionStringName(AbpTreesDemoDbProperties.ConnectionStringName)]
public interface IAbpTreesDemoMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
