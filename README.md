# IczpNet.AbpTrees

An abp module that provides standard tree structure entity implement.

### Create project by Abp Cli

```
abp new IczpNet.AbpTreesDemo -t module --no-ui
```



### Build

```
dotnet build --configuration Release
```

https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build

### Public

```
dotnet nuget push "src/*/bin/Release/*0.1.17.nupkg" --skip-duplicate -k oy2jmdqv6mthwoeqtddq222erkpxkvcejg3eth5sybb4fq --source https://api.nuget.org/v3/index.json
```



## Installation

#### Install the following NuGet packages. (see how)

- IczpNet.AbpTrees.Domain
- IczpNet.AbpTrees.Application
- IczpNet.AbpTrees.Application.Contracts
- IczpNet.AbpTrees.Domain.Shared

#### Add `DependsOn(typeof(AbpTreesXxxModule))` attribute to configure the module dependencies. 

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

   

## Internal structure

### IczpNet.AbpTrees.Domain

#### ITreeEntity

```C#
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace IczpNet.AbpTrees
{
    public interface ITreeEntity<T, TKey> : ITreeEntity<TKey>
        where T : ITreeEntity<TKey>
        where TKey : struct
    {
        T Parent { get; }
        IEnumerable<T> Childs { get; }
        void SetName(string name);
        void SetParent(T parent);
        void SetParentId(TKey? parentId);
    }


    public interface ITreeEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        string Name { get; }
        TKey? ParentId { get; }
        string FullPath { get; }
        string FullPathName { get; }
        int Depth { get; }
        double Sorting { get; set; }
        string Description { get; set; }
    }
}

```



#### TreeEntity

```C#
using IczpNet.AbpTrees.Statics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace IczpNet.AbpTrees
{
    public abstract class TreeEntity<T, TKey> : FullAuditedAggregateRoot<TKey>, ITreeEntity<T, TKey>
        where T : ITreeEntity<TKey>
        where TKey : struct
    {
        [MaxLength(64)]
        [Required(ErrorMessage = "Name Required.")]
        public virtual string Name { get; protected set; }

        public virtual TKey? ParentId { get; set; }

        [MaxLength(1000)]
        [Required]
        public virtual string FullPath { get; protected set; }

        [MaxLength(1000)]
        [Required]
        public virtual string FullPathName { get; protected set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Range(0, 1024)]
        public virtual int Depth { get; protected set; }

        public virtual double Sorting { get; set; }

        [MaxLength(500)]
        public virtual string Description { get; set; }

        public virtual int GetChildsCount()
        {
            return Childs.Count();
        }

        /// <summary>
        /// 父级角色
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public virtual T Parent { get; protected set; }

        /// <summary>
        /// 子集合
        /// </summary>
        public virtual IEnumerable<T> Childs { get; protected set; }

        protected TreeEntity()
        {

        }

        protected TreeEntity(TKey id, string name, TKey? parentId) : base(id)
        {
            SetId(id);
            SetParentId(parentId);
            SetName(name);
            SetFullPath(null);
            SetFullPathName(null);
        }

        public virtual void SetParentId(TKey? parentId)
        {
            ParentId = parentId;
        }

        protected virtual void SetId(TKey id)
        {
            Id = id;
        }

        public virtual void SetName(string name)
        {
            Name = name;
        }

        protected virtual void SetFullPath(string parentPath)
        {
            FullPath = parentPath.IsNullOrEmpty() ? $"{Id}" : $"{parentPath}{AbpTreesConsts.SplitPath}{Id}";
        }

        protected virtual void SetFullPathName(string parentPathName)
        {
            FullPathName = parentPathName.IsNullOrEmpty() ? $"{Name}" : $"{parentPathName}{AbpTreesConsts.SplitPath}{Name}";
        }

        protected virtual void SetDepth(int depth)
        {
            Depth = depth;
        }

        public virtual void SetParent(T parent)
        {
            if (parent == null)
            {
                SetDepth(0);
                SetFullPath(null);
                SetFullPathName(null);
            }
            else
            {
                Parent = parent;
                Assert.If(Parent.Depth >= AbpTreesConsts.MaxDepth, $"超出最大层级:{AbpTreesConsts.MaxDepth}");
                SetDepth(Parent.Depth + 1);
                SetFullPath(parent.FullPath);
                SetFullPathName(parent.FullPathName);
            }
        }
    }
}
```



#### ITreeManager

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace IczpNet.AbpTrees
{
    public interface ITreeManager<T, TKey, TTreeInfo, TWithChildsOuput, TwithParentOuput> : ITreeManager<T, TKey, TTreeInfo, TWithChildsOuput>, IDomainService
        where T : ITreeEntity<TKey>
        where TKey : struct
        where TTreeInfo : ITreeInfo<TKey>
        where TWithChildsOuput : ITreeWithChildsInfo<TWithChildsOuput>
        where TwithParentOuput : ITreeWithParentInfo<TwithParentOuput>
    {
        Task<TwithParentOuput> GetWithParentAsync(TKey id);
    }
    public interface ITreeManager<T, TKey, TTreeInfo, TWithChildsOuput> : ITreeManager<T, TKey, TTreeInfo>, IDomainService
        where T : ITreeEntity<TKey>
        where TKey : struct
        where TTreeInfo : ITreeInfo<TKey>
        where TWithChildsOuput : ITreeWithChildsInfo<TWithChildsOuput>
    {
        Task<List<TWithChildsOuput>> GetAllListWithChildsAsync(TKey? parentId, bool isImportAllChilds = false);
        Task<List<TWithChildsOuput>> GetRootListAsync(List<TKey> idList);
    }
    public interface ITreeManager<T, TKey, TTreeOutput> : ITreeManager<T, TKey>, IDomainService
        where T : ITreeEntity<TKey>
        where TKey : struct
        where TTreeOutput : ITreeInfo<TKey>
    {
        Task<List<TTreeOutput>> GetAllByCacheAsync();
    }

    public interface ITreeManager<T, TKey> : IDomainService
        where T : ITreeEntity<TKey>
        where TKey : struct
    {
        Task RemoveCacheAsync();
        /// <summary>
        /// 查找当前目录及所有子目录
        /// </summary>
        /// <param name="treeEntityIdList"></param>
        /// <returns></returns>
        Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(IEnumerable<TKey> treeEntityIdList);
        /// <summary>
        /// 查找当前目录及所有子目录
        /// </summary>
        /// <param name="treeEntityIdList"></param>
        /// <returns></returns>
        Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(TKey treeEntityIdList);
        /// <summary>
        /// 查找当前目录及所有子目录
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(string fullPath);
        /// <summary>
        /// 查找当前目录及所有子目录
        /// </summary>
        /// <param name="fullPaths"></param>
        /// <returns></returns>
        Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(IEnumerable<string> fullPaths);
        Task<T> FindAsync(TKey id);
        Task<T> GetAsync(TKey id);
        Task<List<T>> GetManyAsync(IEnumerable<TKey> idList);
        //Task<T> CreateAsync(string name, TKey? parentId, long sorting, string description);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(TKey id);
        /// <summary>
        /// 获取子目录
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        Task<List<T>> GetChildsAsync(TKey? entityId);

        Task RepairDataAsync();
    }
}
```

#### TreeManager

```C#
using IczpNet.AbpTrees.Statics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace IczpNet.AbpTrees
{
    public class TreeManager<T, TKey, TOutput, TWithChildsOuput, TWithParentOuput> : TreeManager<T, TKey, TOutput, TWithChildsOuput>, ITreeManager<T, TKey, TOutput, TWithChildsOuput, TWithParentOuput>
         where T : class, ITreeEntity<T, TKey>
        where TKey : struct
        where TOutput : class, ITreeInfo<TKey>
        where TWithChildsOuput : class, ITreeWithChildsInfo<TWithChildsOuput>
        where TWithParentOuput : class, ITreeWithParentInfo<TWithParentOuput>
    {
        public TreeManager(IRepository<T, TKey> repository) : base(repository) { }

        public async Task<TWithParentOuput> GetWithParentAsync(TKey id)
        {
            var entity = await GetAsync(id);
            return ObjectMapper.Map<T, TWithParentOuput>(entity);
        }
    }
    public class TreeManager<T, TKey, TOutput, TWithChildsOuput> : TreeManager<T, TKey, TOutput>, ITreeManager<T, TKey, TOutput, TWithChildsOuput>
         where T : class, ITreeEntity<T, TKey>
        where TKey : struct
        where TOutput : class, ITreeInfo<TKey>
        where TWithChildsOuput : class, ITreeWithChildsInfo<TWithChildsOuput>
    {
        public TreeManager(IRepository<T, TKey> repository) : base(repository) { }

        public override Task RemoveCacheAsync()
        {
            return Cache.RemoveAsync(CacheKey);
        }

        public virtual async Task<List<TWithChildsOuput>> GetAllListWithChildsAsync(TKey? parentId, bool isImportAllChilds = false)
        {
            var allList = await GetAllByCacheAsync();

            return await GetChildsAsync(allList, parentId, isImportAllChilds);
        }

        private async Task<List<TWithChildsOuput>> GetChildsAsync(List<TOutput> allList, TKey? parentId, bool isImportAllChilds)
        {
            var list = new List<TWithChildsOuput>();

            foreach (var treeInfo in allList.Where(x => x.ParentId.Equals(parentId) ).ToList())
            {
                var item = ObjectMapper.Map<TOutput, TWithChildsOuput>(treeInfo);

                if (isImportAllChilds)
                {
                    item.Childs = await GetChildsAsync(allList, treeInfo.Id, isImportAllChilds);
                }
                list.Add(item);
            }
            return list;
        }

        public virtual async Task<List<TWithChildsOuput>> GetRootListAsync(List<TKey> idList)
        {
            var rootList = (await Repository.GetQueryableAsync())
               .Where(x => x.ParentId == null)
               .WhereIf(idList != null && idList.Any(), x => idList.Contains(x.Id))
               .ToList();

            return ObjectMapper.Map<List<T>, List<TWithChildsOuput>>(rootList);
        }
    }

    public class TreeManager<T, TKey, TOutput> : TreeManager<T, TKey>, ITreeManager<T, TKey, TOutput>
        where T : class, ITreeEntity<T, TKey>
        where TKey : struct
        where TOutput : class, ITreeInfo<TKey>
    {

        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
        protected IDistributedCache<List<TOutput>> Cache => LazyServiceProvider.LazyGetRequiredService<IDistributedCache<List<TOutput>>>();

        public TreeManager(IRepository<T, TKey> repository) : base(repository) { }

        public override Task RemoveCacheAsync()
        {
            return Cache.RemoveAsync(CacheKey);
        }

        public virtual Task<List<TOutput>> GetAllByCacheAsync()
        {
            return Cache.GetOrAddAsync(CacheKey, async () =>
            {
                var list = (await Repository.GetQueryableAsync()).OrderByDescending(x => x.Sorting).ToList();

                var result = new List<TOutput>();

                foreach (var item in list)
                {
                    result.Add(ObjectMapper.Map<T, TOutput>(item));
                }
                return await Task.FromResult(result);
            });
        }
    }


    public class TreeManager<T, TKey> : DomainService, ITreeManager<T, TKey>
        where T : class, ITreeEntity<T, TKey>
        where TKey : struct
    {
        public virtual string CacheKey => typeof(T).FullName;
        public IRepository<T, TKey> Repository { get; }
        public TreeManager(IRepository<T, TKey> repository)
        {
            Repository = repository;
        }

        public virtual async Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(IEnumerable<TKey> departmentIdList)
        {
            var fullPathsQueryable = (await Repository.GetQueryableAsync())
                .Where(x => departmentIdList.Contains(x.Id))
                .Select(x => x.FullPath)
            ;

            var fullPathList = await AsyncExecuter.ToListAsync(fullPathsQueryable);

            return await QueryCurrentAndAllChildsAsync(fullPathList);
        }
        public virtual async Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(IEnumerable<string> fullPaths)
        {
            var entityPredicate = PredicateBuilder.New<T>();

            foreach (var fullPath in fullPaths)
            {
                entityPredicate = entityPredicate.Or(x => x.FullPath.StartsWith(fullPath));
            }

            var entityIdQuery = (await Repository.GetQueryableAsync())
                .Where(entityPredicate)
            ;

            //Logger.LogDebug("entityIdQuery:\r\n" + entityIdQuery.ToQueryString());
            //Logger.LogDebug("entityIdQuery:\r\n" + string.Join(",", entityIdQuery.ToList()));

            return entityIdQuery;
        }
        public virtual Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(string fullPath)
        {
            return QueryCurrentAndAllChildsAsync(new List<string>() { fullPath });
        }

        public virtual Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(TKey departmentId)
        {
            return QueryCurrentAndAllChildsAsync(new List<TKey>() { departmentId });
        }

        public virtual Task RemoveCacheAsync()
        {
            //return Cache.RemoveAsync(CacheKey);
            return Task.CompletedTask;
        }

        public virtual Task<T> FindAsync(TKey id)
        {
            return Repository.FindAsync(id);
        }

        public virtual Task<T> GetAsync(TKey id)
        {
            return Repository.GetAsync(id);
        }

        public virtual Task<List<T>> GetManyAsync(IEnumerable<TKey> idList)
        {
            return Repository.GetListAsync(x => idList.Contains(x.Id));
        }

        public virtual async Task<T> CreateAsync(T inputEntity)
        {
            Assert.If(await Repository.CountAsync(x => x.Name == inputEntity.Name) > 0, $"Already exists:{inputEntity.Name}");

            if (inputEntity.ParentId.HasValue)
            {
                var parent = await Repository.GetAsync(inputEntity.ParentId.Value);

                Assert.NotNull(parent, $"No such parent entity:{inputEntity.ParentId}");

                inputEntity.SetParent(parent);
            }
            else
            {
                inputEntity.SetParent(null);
            }

            var entity = await Repository.InsertAsync(inputEntity, autoSave: true);

            await RemoveCacheAsync();

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            Assert.NotNull(entity, $"an entity is no such.");

            Assert.NotNull(entity.Name, $"[Name] cannot be null.");

            Assert.If(entity.Name.Contains(AbpTreesConsts.SplitPath), $"[Name] cannot contains char:\"/\"");

            Assert.If(await Repository.CountAsync(x => x.Name == entity.Name && !x.Id.Equals(entity.Id)) > 0, $" Name[{entity.Name}] already such.");

            //entity.SetName(entity.Name);

            if (entity.ParentId.HasValue)
            {
                //变更上级
                var parent = await Repository.GetAsync(entity.ParentId.Value);

                Assert.NotNull(parent, $"[Parent] is no such.");

                entity.SetParent(parent);
            }
            else
            {
                entity.SetParent(null);
            }

            //update childs
            await ChangeChildsAsync(entity);

            await RemoveCacheAsync();

            return entity;
        }

        protected virtual async Task ChangeChildsAsync(T entiy)
        {
            Logger.LogInformation($"ChangeChilds id:{entiy.Id}");

            foreach (var item in entiy.Childs)
            {
                item.SetParent(entiy);

                await ChangeChildsAsync(item);
            }
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await Repository.GetAsync(id);

            var childCount = entity.Childs.Count();

            Assert.If(childCount > 0, $"Has ({childCount}) childs, cannot delete.");

            await Repository.DeleteAsync(entity);

            await RemoveCacheAsync();
        }

        public async Task<List<T>> GetChildsAsync(TKey? entityId)
        {
            //return await Repository.GetListAsync(x => x.ParentId == departmentId);
            return (await Repository.GetQueryableAsync())
                .Where(x => x.ParentId.Equals(entityId))
                .OrderByDescending(x => x.Sorting)
                .ToList();
        }

        public virtual async Task RepairDataAsync()
        {
            var list = await Repository.GetListAsync(x => x.ParentId == null);

            foreach (var entity in list)
            {
                await SetEntityAsync(entity);

                await UpdateAsync(entity);
            }
        }

        protected virtual Task SetEntityAsync(T entity)
        {
            Logger.LogInformation($"SetEntityAsync:{entity}");

            entity.SetName(entity.Name);

            return Task.CompletedTask;
        }
    }
}
```

### IczpNet.AbpTrees.Application.Contracts

#### ITreeAppService

```C#
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace IczpNet.AbpTrees
{
    public interface ITreeAppService<TGetOutputDto, TGetListOutputDto, TKey, in TGetListInput, in TCreateInput, in TUpdateInput, TTreeInfo>
        : ITreeAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TKey : struct
        where TTreeInfo : ITreeInfo<TKey>
    {
        Task<TTreeInfo> GetItemByCacheAsync(TKey id);

        Task<List<TTreeInfo>> GetManayByCacheAsync(List<TKey> idList);

        Task<List<TTreeInfo>> GetAllByCacheAsync();
    }

    public interface ITreeAppService<TGetOutputDto, TGetListOutputDto, TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TKey : struct
    {

        Task<List<TGetOutputDto>> GetManyAsync(List<TKey> idList);

        Task<DateTime> RepairDataAsync();


    }
}


```

### Dtos

#### ITreeGetListInput

```C#
namespace IczpNet.AbpTrees.Dtos
{
    public interface ITreeGetListInput<TKey> where TKey : struct
    {
        bool IsEnabledParentId { get; set; }

        int? Depth { get; set; }

        TKey? ParentId { get; set; }

        string Keyword { get; set; }
    }
}

```

#### ITreeInput

```C#
using System;

namespace IczpNet.AbpTrees.Dtos
{
    public interface ITreeInput<TKey> where TKey : struct
    {
        string Name { get; set; }
        TKey? ParentId { get; set; }
    }
}
```

#### TreeGetListInput

```C#
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTrees.Dtos
{
    public class TreeGetListInput<TKey> : PagedAndSortedResultRequestDto, ITreeGetListInput<TKey> where TKey : struct
    {
        [DefaultValue(false)]
        public virtual bool IsEnabledParentId { get; set; }

        [DefaultValue(null)]
        public virtual int? Depth { get; set; }

        [DefaultValue(null)]
        public virtual TKey? ParentId { get; set; }

        [DefaultValue(null)]
        public virtual string Keyword { get; set; }
    }
}

```

### IczpNet.AbpTrees.Application

#### TreeAppService

```C#
using IczpNet.AbpTrees.Dtos;
using IczpNet.AbpTrees.Statics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTrees
{

    public abstract class TreeAppService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput, TTreeInfo> :
        TreeAppService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput>,
        ITreeAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput, TTreeInfo>
        where TEntity : class, ITreeEntity<TEntity, TKey>
        where TKey : struct
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
        where TGetListInput : ITreeGetListInput<TKey>
        where TCreateInput : ITreeInput<TKey>
        where TUpdateInput : ITreeInput<TKey>
        where TTreeInfo : ITreeInfo<TKey>

    {
        protected ITreeManager<TEntity, TKey, TTreeInfo> TreeCacheManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TKey, TTreeInfo>>();
        protected TreeAppService(IRepository<TEntity, TKey> repository) : base(repository) { }

        [HttpGet]
        public virtual Task<TTreeInfo> GetItemByCacheAsync(TKey id)
        {
            return TreeCacheManager.GetItemByCacheAsync(id);
        }

        [HttpGet]
        public virtual Task<List<TTreeInfo>> GetManayByCacheAsync(List<TKey> idList)
        {
            return TreeCacheManager.GetManyByCacheAsync(idList);
        }

        [HttpGet]
        public virtual async Task<List<TTreeInfo>> GetAllByCacheAsync()
        {
            await CheckGetListPolicyAsync();

            return await TreeCacheManager.GetAllByCacheAsync();
        }
    }


    public abstract class TreeAppService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput> :
        CrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        ITreeAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, ITreeEntity<TEntity, TKey>
        where TKey : struct
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
        where TGetListInput : ITreeGetListInput<TKey>
        where TCreateInput : ITreeInput<TKey>
        where TUpdateInput : ITreeInput<TKey>

    {
        protected virtual string RepairDataPolicyName { get; set; }

        protected virtual ITreeManager<TEntity, TKey> TreeManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TKey>>();

        public TreeAppService(IRepository<TEntity, TKey> repository) : base(repository) { }

        protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
        {
            return query.OrderByDescending(x => x.Sorting);
        }

        [HttpGet]
        public override Task<TGetOutputDto> GetAsync(TKey id)
        {
            return base.GetAsync(id);
        }



        [HttpGet]
        public virtual async Task<List<TGetOutputDto>> GetManyAsync(List<TKey> idList)
        {
            var list = new List<TGetOutputDto>();
            foreach (var id in idList)
            {
                list.Add(await GetAsync(id));
            }
            return list;
        }

        [HttpGet]
        public override Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {
            return base.GetListAsync(input);
        }

        protected override async Task<IQueryable<TEntity>> CreateFilteredQueryAsync(TGetListInput input)
        {
            Assert.If(!input.IsEnabledParentId && input.ParentId != null, "When [IsEnabledParentId]=false,then [ParentId] != null");

            return (await base.CreateFilteredQueryAsync(input))
                .WhereIf(input.DepthList != null && input.DepthList.Any(), x => input.DepthList.Contains(x.Depth))
                .WhereIf(input.IsEnabledParentId, x => x.ParentId.Equals(input.ParentId))
               //.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword))
               ;
        }


        [HttpPost]
        public override async Task<TGetOutputDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePolicyAsync();

            var inputEntity = MapToEntity(input);

            inputEntity.SetName(input.Name);

            inputEntity.SetParentId(input.ParentId);

            var entity = await TreeManager.CreateAsync(inputEntity);

            return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
        }

        [HttpPost]
        public override async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await GetEntityByIdAsync(id);

            await MapToEntityAsync(input, entity);

            entity.SetName(input.Name);

            entity.SetParentId(input.ParentId);

            await TreeManager.UpdateAsync(entity);

            return await MapToGetOutputDtoAsync(entity);
        }

        [HttpPost]
        public override async Task DeleteAsync(TKey id)
        {
            await CheckDeletePolicyAsync();

            await TreeManager.DeleteAsync(id);
        }

        [HttpPost]
        public virtual async Task<DateTime> RepairDataAsync()
        {
            await CheckRepairDataPolicyAsync();

            await TreeManager.RepairDataAsync();

            return Clock.Now;
        }

        protected virtual async Task CheckRepairDataPolicyAsync()
        {
            await CheckPolicyAsync(RepairDataPolicyName);
        }
    }
}

```



## Usage

https://github.com/Iczp/AbpTrees/tree/master/Example

### Create a entity

1. Create a entity [`Department`] and implement `TreeEntity<T>`.

   ```c#
   using IczpNet.AbpTrees;
   using System;
   
   namespace IczpNet.AbpTreesDemo.Departments
   {
       public class Department : TreeEntity<Department, Guid>
       {
       }
   }
   
   ```

### Create  Model

   1. Create  `DepartmentInfo`  and implement `TreeInfo` in project `IczpNet.AbpTreesDemo.Domain.Shared`

   ```C#
   using IczpNet.AbpTrees;
   using System;
   
   namespace IczpNet.AbpTreesDemo.Departments
   {
       public class DepartmentInfo : TreeInfo<Guid>
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
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentCreateInput
/// </summary>
public class DepartmentCreateInput : DepartmentUpdateInput, ITreeInput<Guid>
{

}

```

2. `DepartmentDto.cs`

```C#
using System;
using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTreesDemo.Departments.Dtos
{
    public class DepartmentDto : DepartmentInfo, IEntityDto<Guid>
    {
        public virtual double Sorting { get; set; }
        public virtual string Description { get; set; }
    }
}

```

3. `DepartmentGetAllListWithChildsInput.cs`

```C#
using System;
using System.ComponentModel;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentGetAllListWithChildsInput 
{
    [DefaultValue(null)]
    public virtual Guid? ParentId { get; set; }
    public virtual bool IsImportAllChilds { get; set; }
}

```

4. `DepartmentGetListInput.cs`

```C#
using IczpNet.AbpTrees.Dtos;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentGetListInput : TreeGetListInput<Guid>
{

}

```

5. `DepartmentUpdateInput.cs`

```C#
using IczpNet.AbpTrees.Dtos;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentUpdateInput : ITreeInput<Guid>
{
    public virtual Guid? ParentId { get; set; }
    public virtual string Name { get; set; }
    public virtual double Sorting { get; set; }
    public virtual string Description { get; set; }

}

```

6. `DepartmentWithChildsDto.cs`

```C#
using IczpNet.AbpTrees;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentWithChildsDto : TreeWithChildsInfo<DepartmentWithChildsDto, Guid>
{
    public virtual int ChildsCount { get; set; }
}

```

7. `DepartmentWithParentDto.cs`

```C#
using IczpNet.AbpTrees;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentWithParentDto : TreeWithParentInfo<DepartmentWithParentDto, Guid>
{
    public virtual double Sorting { get; set; }
    public virtual string Description { get; set; }
}

```



### interface CRUD

IDepartmentAppSevice and implement  `ICrudAppService`,  `ITreeAppService`

```C#
using IczpNet.AbpTrees;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using System;

namespace IczpNet.AbpTreesDemo.Departments
{
    public interface IDepartmentAppSevice :
        ITreeAppService<DepartmentDto,
            DepartmentDto,
            Guid,
            DepartmentGetListInput,
            DepartmentCreateInput,
            DepartmentUpdateInput, DepartmentInfo>
    {
    }
}


```

### Application CRUD

`IczpNet.AbpTreesDemo.Application`  > `DepartmentAppsevice.cs`

```C#
using IczpNet.AbpTrees;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTreesDemo.Departments
{

    [Route($"Api/App/{AbpTreesDemoRemoteServiceConsts.ModuleName}/[Controller]/[Action]")]
    public class DepartmentAppService
        : TreeAppService<
            Department,
            Guid,
            DepartmentDto,
            DepartmentDto,
            DepartmentGetListInput,
            DepartmentCreateInput,
            DepartmentUpdateInput,
            DepartmentInfo>,
        IDepartmentAppSevice
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

   

## Run

   1. Set as Startup Project:`IczpNet.AbpTreesDemo.HttpApi.Host`

   2. ConnectionStrings:`appsettings.json`

      ```json
      {
        "App": {
          "CorsOrigins": "https://*.AbpTreesDemo.com,http://localhost:4200,http://localhost:44307,https://localhost:44307"
        },
        "ConnectionStrings": {
          "Default": "Server=localhost;Initial Catalog=AbpTreesDemo_Main;User ID=sa;Password=123;TrustServerCertificate=True",
          "AbpTreesDemo": "Server=localhost;Initial Catalog=AbpTreesDemo_Module;User ID=sa;Password=123;TrustServerCertificate=True"
        },
        "Redis": {
          "Configuration": "127.0.0.1"
        },
        "AuthServer": {
          "Authority": "https://localhost:44362/",
          "RequireHttpsMetadata": "false",
          "SwaggerClientId": "AbpTreesDemo_Swagger",
          "SwaggerClientSecret": "1q2w3e*"
        }
      }
      
      
      ```

      

   3. Set PM(Package Management Console) default Project:`IczpNet.AbpTreesDemo.HttpApi.Host`

   4. add-migration and update database

      ```
      PM> Add-Migration Department_Init
      ```

      ```
      PM> Update-Database
      ```

      

### Upgrade v1.0.13(Add Property ChildrenCount)

```sql
  --update ChildrenCount
  
  UPDATE [dbo].[AbpTreesDemoDepartment] 
  SET [dbo].[AbpTreesDemoDepartment].ChildrenCount = (
  SELECT COUNT(1) FROM [dbo].[AbpTreesDemoDepartment] WHERE ParentId=x.Id
  )
  FROM [dbo].[AbpTreesDemoDepartment] x
```

