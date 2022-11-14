using IczpNet.AbpTreesDemo.Departments;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace IczpNet.AbpTreesDemo.EntityFrameworkCore;

public static class AbpTreesDemoDbContextModelCreatingExtensions
{
    public static void ConfigureAbpTreesDemo(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(AbpTreesDemoDbProperties.DbTablePrefix + "Questions", AbpTreesDemoDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        builder.Entity<Department>(b =>
        {
            //Configure table & schema name
            b.ToTable(AbpTreesDemoDbProperties.DbTablePrefix + nameof(Department), AbpTreesDemoDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Indexes
            b.HasIndex(q => q.CreationTime);

        });
    }
}
