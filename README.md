#  IczpNet.AbpTrees



### Create project by Abp Cli

```
abp new IczpNet.AbpTreesDemo -t module --no-ui
```





An abp module that provides standard tree structure entity implement.

## Installation

### Install the following NuGet packages. (see how)

- IczpNet.AbpTrees.Domain
- IczpNet.AbpTrees.Application
- IczpNet.AbpTrees.Application.Contracts
- IczpNet.AbpTrees.Domain.Shared

## Add `DependsOn(typeof(AbpTreesXxxModule))` attribute to configure the module dependencies. 

1. ### `IczpNet.AbpTreesDemo.Domain` 

   `F:\Dev\abpvnext\Iczp.AbpTrees\Example\src\IczpNet.AbpTreesDemo.Domain\AbpTreesDemoDomainModule.cs`

   ```c#
   [DependsOn(typeof(AbpTreesDomainModule))]
   ```

2. ###  `IczpNet.AbpTreesDemo.Domain.Shared`

   ```c#
   [DependsOn(typeof(AbpTreesDomainSharedModule))]
   ```


5. ### `IczpNet.AbpTreesDemo.Application.Contracts`

   ```c#
   [DependsOn(typeof(AbpTreesApplicationContractsModule))]
   ```

6. ###  `IczpNet.AbpTreesDemo.Application`

   ```c#
   [DependsOn(typeof(AbpTreesApplicationModule))]
   ```

   

## Usage

### Create a entity

1. Create a entity [`Department`] and implement `TreeEntity<T>`.

   ```c#
   using IczpNet.AbpTrees;
   
   namespace IczpNet.AbpTreesDemo.Departments
   {
       public class Department : TreeEntity<Department>
       {
       }
   }
   
   ```

### Create  Model

   1. Create  `DepartmentInfo`  and implement `TreeInfo` in project `IczpNet.AbpTreesDemo.Domain.Shared`

   ```C#
   using IczpNet.AbpTrees;
   
   namespace IczpNet.AbpTreesDemo.Departments
   {
       public class DepartmentInfo : TreeInfo
       {
       }
   }
   
   ```

   ### Repository

   1. `IczpNet.AbpTreesDemo.EntityFrameworkCore`   `AbpTreesDemoDbContext.cs`

   ```
   public DbSet<Department> Department { get; }
   ```

   ```C#
   using IczpNet.AbpTreesDemo.Departments;
   using Microsoft.EntityFrameworkCore;
   using Volo.Abp.Data;
   using Volo.Abp.EntityFrameworkCore;
   
   namespace IczpNet.AbpTreesDemo.EntityFrameworkCore;
   
   [ConnectionStringName(AbpTreesDemoDbProperties.ConnectionStringName)]
   public class AbpTreesDemoDbContext : AbpDbContext<AbpTreesDemoDbContext>, IAbpTreesDemoDbContext
   {
       /* Add DbSet for each Aggregate Root here. Example:
        * public DbSet<Question> Questions { get; set; }
        */
   
       public AbpTreesDemoDbContext(DbContextOptions<AbpTreesDemoDbContext> options)
           : base(options)
       {
   
       }
       /// <summary>
       /// Department
       /// </summary>
       public DbSet<Department> Department { get; }
   
       protected override void OnModelCreating(ModelBuilder builder)
       {
           base.OnModelCreating(builder);
   
           builder.ConfigureAbpTreesDemo();
       }
   }
   
   ```

2.  `AbpTreesDemoDbContextModelCreatingExtensions.cs`

   ```C#
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
   
   ```





### Create Dto	

`IczpNet.AbpTreesDemo.Application.Contracts`

1. `DepartmentCreateInput`

```C#
using IczpNet.AbpTrees.Dtos;
namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentCreateInput
/// </summary>
public class DepartmentCreateInput : DepartmentUpdateInput, ITreeInput
{

}

```

2. `DepartmentDto.cs`

```C#
using IczpNet.AbpTreesDemo.Departments;
using System;
using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTreesDemo.Departments.Dtos
{
    /// <summary>
    /// DepartmentDto
    /// </summary>
    public class DepartmentDto : DepartmentInfo, IEntityDto<Guid>
    {
        /// <summary>
        /// 排序（越大越前面） DESC
        /// </summary>
        public virtual long Sorting { get; set; }
        /// <summary>
        /// 说明
        /// </summary>

        public virtual string Description { get; set; }
    }
}

```

3. `DepartmentGetAllListWithChildsInput.cs`

```C#
using System;
using System.ComponentModel;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentGetListInput
/// </summary>
public class DepartmentGetAllListWithChildsInput 
{

    /// <summary>
    /// 上级部门
    /// </summary>
    [DefaultValue(null)]
    public virtual Guid? ParentId { get; set; }
    /// <summary>
    /// 是否包含所有子集
    /// </summary>
    public virtual bool IsImportAllChilds { get; set; }
}

```

4. `DepartmentGetListInput.cs`

```C#
using IczpNet.AbpTrees;
using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentGetListInput
/// </summary>
public class DepartmentGetListInput : PagedAndSortedResultRequestDto, ITreeGetListInput
{
    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(false)]
    public virtual bool IsEnabledParentId { get; set; }
    /// <summary>
    /// 层级
    /// </summary>
    [DefaultValue(null)]
    public virtual int? Depth { get; set; }
    /// <summary>
    /// 上级部门
    /// </summary>
    [DefaultValue(null)]
    public virtual Guid? ParentId { get; set; }
    /// <summary>
    /// 关键字(支持拼音)
    /// </summary>
    [DefaultValue(null)]
    public virtual string Keyword { get; set; }
}

```

5. `DepartmentUpdateInput.cs`

```C#
using IczpNet.AbpTrees.Dtos;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentUpdateInput
/// </summary>
public class DepartmentUpdateInput : ITreeInput
{

    /// <summary>
    /// 上级部门
    /// </summary>
    public virtual Guid? ParentId { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; set; }
    /// <summary>
    /// 排序（越大越前面） DESC
    /// </summary>
    public virtual long Sorting { get; set; }
    /// <summary>
    /// 说明
    /// </summary>
    public virtual string Description { get; set; }

}

```

6. `DepartmentWithChildsDto.cs`

```C#
using IczpNet.AbpTrees;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentWithChildsDto
/// </summary>
public class DepartmentWithChildsDto : TreeWithChildsInfo<DepartmentWithChildsDto>
{
    public virtual int ChildsCount { get; set; }
}

```

7. `DepartmentWithParentDto.cs`

```C#
using IczpNet.AbpTrees;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentWithParentDto
/// </summary>
public class DepartmentWithParentDto : TreeWithParentInfo<DepartmentWithParentDto>
{
    /// <summary>
    /// 排序（越大越前面） DESC
    /// </summary>
    public virtual long Sorting { get; set; }
    /// <summary>
    /// 说明
    /// </summary>
    public virtual string Description { get; set; }
}

```



### interface CRUD

IDepartmentAppSevice and implement  `ICrudAppService`,  `ITreeAppService`

```C#
using IczpNet.AbpTrees;
using System;
using Volo.Abp.Application.Services;

namespace IczpNet.AbpTreesDemo.Departments
{
    public interface IDepartmentAppSevice<TGetOutputDto, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput, TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto>
        : ICrudAppService<TGetOutputDto, TGetListOutputDto, Guid, TGetListInput, TCreateInput, TUpdateInput>
        , ITreeAppService<TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto>
         where TTreeInfo : ITreeInfo
        where TTreeWithChildsDto : ITreeWithChildsInfo<TTreeWithChildsDto>
        where TTreeWithParentDto : ITreeWithParentInfo<TTreeWithParentDto>
    {
    }
}

```

### Application CRUD

`IczpNet.AbpTreesDemo.Application`  > `DepartmentAppsevice.cs`

```C#
using IczpNet.AbpTrees;
using IczpNet.AbpTreesDemo.Departments;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using System;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTreesDemo.Departments
{
    public class DepartmentAppService : TreeAppService<Department, DepartmentDto, DepartmentDto, DepartmentGetListInput, DepartmentCreateInput, DepartmentUpdateInput, DepartmentInfo, DepartmentWithChildsDto, DepartmentWithParentDto>, IDepartmentAppSevice<DepartmentDto, DepartmentDto, DepartmentGetListInput, DepartmentCreateInput, DepartmentUpdateInput, DepartmentInfo, DepartmentWithChildsDto, DepartmentWithParentDto>
    {
        public DepartmentAppService(IRepository<Department, Guid> repository) : base(repository)
        {
        }
    }
}

```

### Dto Mapper

`AbpTreesDemoApplicationAutoMapperProfile`

```C#
using AutoMapper;
using IczpNet.AbpTreesDemo.Departments;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using Volo.Abp.AutoMapper;

namespace IczpNet.AbpTreesDemo;

public class AbpTreesDemoApplicationAutoMapperProfile : Profile
{
    public AbpTreesDemoApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Department, DepartmentDto>(MemberList.Destination);
        CreateMap<Department, DepartmentWithParentDto>(MemberList.Destination);
        CreateMap<Department, DepartmentWithChildsDto>(MemberList.Destination)
             .ForMember(s => s.ChildsCount, map => map.MapFrom(d => d.GetChildsCount()))
             //.ForMember(s => s.UserCount, map => map.MapFrom(d => d.GetUserCount()))
             ;
        CreateMap<DepartmentCreateInput, Department>(MemberList.Source).IgnoreAllPropertiesWithAnInaccessibleSetter();
        CreateMap<DepartmentUpdateInput, Department>(MemberList.Source).IgnoreAllPropertiesWithAnInaccessibleSetter();


        CreateMap<Department, DepartmentInfo>();
        CreateMap<DepartmentInfo, DepartmentWithChildsDto>()
            .Ignore(x => x.ChildsCount)
            .Ignore(x => x.Childs);
    }
}

```



### Add-Migration `IczpNet.AbpTreesDemo.HttpApi.Host`

1. Select Project `IczpNet.AbpTreesDemo.HttpApi.Host`, Set Run Start.

2. Open PM

   ```bash
   PM> Add-Migration Department_Init
   ```

   ```bash
   PM> Update-Database
   ```

3. Add Controller `AbpTreesDemoHttpApiHostModule.cs`

   ```C#
   //...
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
   //...
           Configure<AbpAspNetCoreMvcOptions>(options =>
           {
               options
                   .ConventionalControllers
                   .Create(typeof(AbpTreesDemoApplicationModule).Assembly);
           });
     //...
   }
           
   //...
   ```

   

   