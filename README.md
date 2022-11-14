#  IczpNet.AbpTrees



### Create project by Abp Cli

```
abp new IczpNet.AbpTreesDemo -t module --no-ui
```





An abp module that provides standard tree structure entity implement.

## Installation

1. #### Install the following NuGet packages. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/Hocs/ow-To.md#add-nuget-packages))

   - IczpNet.AbpTrees.Domain
   - IczpNet.AbpTrees.Application
   - IczpNet.AbpTrees.Application.Contracts
   - IczpNet.AbpTrees.Domain.Shared

2. #### Add `DependsOn(typeof(AbpTreesXxxModule))` attribute to configure the module dependencies. ([see how](https://github.com/EasyAbp/EasyAbpGuide/blob/master/docs/How-To.md#add-module-dependencies))

3. #### `IczpNet.AbpTreesDemo.Domain` 

   `F:\Dev\abpvnext\Iczp.AbpTrees\Example\src\IczpNet.AbpTreesDemo.Domain\AbpTreesDemoDomainModule.cs`

   ```c#
   [DependsOn(typeof(AbpTreesDomainModule))]
   ```

4. ####  `IczpNet.AbpTreesDemo.Domain.Shared`

   ```c#
   [DependsOn(typeof(AbpTreesDomainSharedModule))]
   ```


5. `IczpNet.AbpTreesDemo.Application.Contracts`

   ```c#
   [DependsOn(typeof(AbpTreesApplicationContractsModule))]
   ```

   

6.  `IczpNet.AbpTreesDemo.Application`

   ```c#
   [DependsOn(typeof(AbpTreesApplicationModule))]
   ```

   

## Usage

1. Create a entity [Department] and implement `TreeEntity<T>`.

   ```c#
   using IczpNet.AbpTrees;
   
   namespace IczpNet.AbpTreesDemo.Departments
   {
       public class Department : TreeEntity<Department>
       {
       }
   }
   
   ```

   

2. Create a Repository for the entity. `EfCoreTreeRepository<TDbContext, TEntity>` override some function of `EfCoreRepository<TDbContext, TEntity, TKey>` to match tree structure:

   - `InsertAsync` :Auto Append node `Code` and Calc `Level` property when insert
   - `UpdateAsync` :Auto Move node when update a `Entity` that parentId is modified
   - `DeleteAsync` :Also delete `Children` nodes

3. You have two ways to use this `Repository`

   - Way 1 : Default Repository(`ITreeRepository<>`),
     Add `context.Services.AddTreeRepository<MyProjectNameDbContext>();` to ConfigureServices method in `MyProjectNameEntityFrameworkCoreModule.cs`.
   - Way 2 : Create a `CustomRepository` that base on `EfCoreTreeRepository<TDbContext, TEntity>`
   - Example:

   ```
   context.Services.AddAbpDbContext<TestDbContext>(options =>
   {
   	options.AddDefaultRepositories(includeAllEntities: true);//add Abp's `IRepository<TEntity>`
   	options.AddDefaultTreeRepositories();//add `ITreeRepository<TEntity>` for all Entity with implement `ITree<TEntity>`
   	options.TreeEntity<Resource>(x => x.CodeLength = 10);//set CodeLength for each Entity(Default:5)
   });
   ```

## Sample

It works fine with `Volo.Abp.Application.Services.CrudAppService`.

After replacing `IRepository<>` with `ITreeRepository<Domain.OrganizationUnit>`, the repository will handle the tree structure of the entity during creating, updating, and deleting.

```
    public class OrganizationUnitAppService:
        Volo.Abp.Application.Services.CrudAppService<
            Domain.OrganizationUnit, Application.OrganizationUnitDto,
            Application.OrganizationUnitDto,Guid, Volo.Abp.Application.Dtos.IPagedAndSortedResultRequest,
            Application.CreateOrganizationUnitDto,Application.UpdateOrganizationUnitDto>,
        IOrganizationUnitAppService
        
    {
        public OrganizationUnitAppService(
            IczpNet.AbpTrees.ITreeRepository<Domain.OrganizationUnit> organizationUnitRepository
            ):base(organizationUnitRepository)
        {
            
        }

    }
```

## 