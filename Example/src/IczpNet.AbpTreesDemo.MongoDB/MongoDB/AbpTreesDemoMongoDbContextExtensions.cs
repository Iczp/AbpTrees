using Volo.Abp;
using Volo.Abp.MongoDB;

namespace IczpNet.AbpTreesDemo.MongoDB;

public static class AbpTreesDemoMongoDbContextExtensions
{
    public static void ConfigureAbpTreesDemo(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
