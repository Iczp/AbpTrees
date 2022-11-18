# IczpNet.AbpTrees



### Create project by Abp Cli

```
abp new IczpNet.AbpTreesDemo -t module --no-ui
```





An abp module that provides standard tree structure entity implement.

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
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace IczpNet.AbpTrees
{
    public interface ITreeEntity<T> : ITreeEntity where T : ITreeEntity
    {

        T Parent { get; }
        IEnumerable<T> Childs { get; }
        void FillCreate(Guid id,string name, Guid? parentId);
        //void SetParent(T parent);
        //void SetId(Guid id);
    }


    public interface ITreeEntity : IEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        ///// <summary>
        ///// 名称_拼音
        ///// </summary>
        //string Name_Pinyin { get; }

        ///// <summary>
        ///// 名称_拼音
        ///// </summary>
        //string Name_PY { get; }

        /// <summary>
        /// 父级Id
        /// </summary>
        Guid? ParentId { get; }

        /// <summary>
        /// 全路径
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// 全路径名称
        /// </summary>
        string FullPathName { get; }

        ///// <summary>
        ///// 全路径拼音
        ///// </summary>
        //string FullPathPinyin { get; }

        /// <summary>
        /// 层级
        /// </summary>
        int Depth { get; }

        /// <summary>
        /// 排序（越大越前面） DESC
        /// </summary>
        double Sorting { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        string Description { get; set; }
    }
}

```



#### TreeEntity

```C#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using IczpNet.AbpTrees.Statics;
using Volo.Abp.Domain.Entities.Auditing;

namespace IczpNet.AbpTrees
{
    public abstract class TreeEntity<T> : FullAuditedAggregateRoot<Guid>, ITreeEntity<T> where T : ITreeEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(64)]
        [Required(ErrorMessage = "名称不能为NUll")]
        public virtual string Name { get; protected set; }

        ///// <summary>
        ///// 名称_拼音
        ///// </summary>
        //[StringLength(AbpTreeConsts.Name_PinyinMaxLength)]
        //[MaxLength(AbpTreeConsts.Name_PinyinMaxLength)]
        //// [Comment("名称_拼音")]
        //public virtual string Name_Pinyin { get; protected set; }

        ///// <summary>
        ///// 名称_拼音
        ///// </summary>
        //[StringLength(AbpTreeConsts.Name_PYMaxLength)]
        //[MaxLength(AbpTreeConsts.Name_PYMaxLength)]
        //// [Comment("名称_拼音")]
        //public virtual string Name_PY { get; protected set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        // [Comment("父级Id")]
        public virtual Guid? ParentId { get; protected set; }

        /// <summary>
        /// 全路径
        /// </summary>
        [MaxLength(1000)]
        [Required]
        // [Comment("全路径")]
        public virtual string FullPath { get; protected set; }

        /// <summary>
        /// 全路径名称
        /// </summary>
        [MaxLength(1000)]
        [Required]
        // [Comment("全路径名称")]
        public virtual string FullPathName { get; protected set; }

        ///// <summary>
        ///// 全路径拼音
        ///// </summary>
        //[MaxLength(1000)]
        //[Required]
        //// [Comment("全路径拼音")]
        //public virtual string FullPathPinyin { get; protected set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Range(0, 16)]
        // [Comment("层级")]
        public virtual int Depth { get; protected set; }

        /// <summary>
        /// 排序（越大越前面） DESC
        /// </summary>
        // [Comment("排序（越大越前面） DESC")]
        public virtual double Sorting { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [MaxLength(500)]
        // [Comment("说明")]
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

        protected TreeEntity(Guid id, string name, Guid? parentId) : base(id)
        {
            FillCreate(id, name, parentId);
        }

        public virtual void FillCreate(Guid id, string name, Guid? parentId)
        {
            SetId(id);
            SetParentId(parentId);
            SetName(name);
            SetFullPath(null);
            SetFullPathName(null);
            //SetFullPathPinyin(null);
        }

        protected virtual void SetParentId(Guid? parentId)
        {
            ParentId = parentId;
        }

        protected virtual void SetId(Guid id)
        {
            Id = id;
        }
        public virtual void SetName(string name)
        {
            Assert.NotNull(name, $"名称不能为Null");

            Assert.If(name.Contains(AbpTreesConsts.SplitPath), $"名称不能包含\"/\"");

            Name = name;

            //Name_PY = name.ConvertToPY().MaxLength(300);

            //Name_Pinyin = name.ConvertToPinyin().MaxLength(300);
        }

        public virtual void SetFullPath(string parentPath)
        {
            FullPath = parentPath.IsNullOrEmpty() ? $"{Id}" : $"{parentPath}{AbpTreesConsts.SplitPath}{Id}";
        }

        public virtual void SetFullPathName(string parentPathName)
        {
            FullPathName = parentPathName.IsNullOrEmpty() ? $"{Name}" : $"{parentPathName}{AbpTreesConsts.SplitPath}{Name}";
        }

        //internal virtual void SetFullPathPinyin(string parentPathPinyin)
        //{
        //    FullPathPinyin = parentPathPinyin.IsNullOrEmpty() ? $"{Name_PY}" : $"{parentPathPinyin}{AbpTreeConsts.SplitPath}{Name_PY}";
        //}

        protected virtual void SetDepth(int depth)
        {
            Depth = depth;
        }


        public virtual void SetParent(T parent)
        {
            if (parent == null)
            {
                SetFullPath(null);
                SetFullPathName(null);
                //SetFullPathPinyin(null);
                return;
            }
            Parent = parent;

            Assert.If(Parent.Depth >= AbpTreesConsts.MaxDepth, $"超出最大层级:{AbpTreesConsts.MaxDepth}");

            SetDepth(Parent.Depth + 1);
            SetFullPath(parent.FullPath);
            SetFullPathName(parent.FullPathName);
            //SetFullPathPinyin(parent.FullPathPinyin);
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
    //public interface ITreeManager<T, TTreeInfo, TWithChildsOuput, TwithParentOuput> : ITreeManager<T, TTreeInfo, TWithChildsOuput>, IDomainService
    //    where T : ITreeEntity<T>
    //    where TTreeInfo : ITreeInfo
    //    where TWithChildsOuput : ITreeWithChildsInfo<TWithChildsOuput>
    //    where TwithParentOuput : ITreeWithParentInfo<TwithParentOuput>
    //{
    //    Task<TwithParentOuput> GetWithParentAsync(Guid id);
    //}
    public interface ITreeManager<T, TTreeInfo, TWithChildsOuput> : ITreeManager<T, TTreeInfo>, IDomainService
        where T : ITreeEntity<T>
        where TTreeInfo : ITreeInfo
        where TWithChildsOuput : ITreeWithChildsInfo<TWithChildsOuput>
    {
        Task<List<TWithChildsOuput>> GetAllListWithChildsAsync(Guid? parentId, bool isImportAllChilds = false);
        Task<List<TWithChildsOuput>> GetRootListAsync(List<Guid> idList);
    }
    public interface ITreeManager<T, TTreeOutput> : ITreeManager<T>, IDomainService
        where T : ITreeEntity<T>
        where TTreeOutput : ITreeInfo
    {
        Task<List<TTreeOutput>> GeAllListByCacheAsync();
    }

    public interface ITreeManager<T> : IDomainService where T : ITreeEntity<T>
    {
        Task RemoveCacheAsync();
        /// <summary>
        /// 查找当前目录及所有子目录
        /// </summary>
        /// <param name="treeEntityIdList"></param>
        /// <returns></returns>
        Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(IEnumerable<Guid> treeEntityIdList);
        /// <summary>
        /// 查找当前目录及所有子目录
        /// </summary>
        /// <param name="treeEntityIdList"></param>
        /// <returns></returns>
        Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(Guid treeEntityIdList);
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
        Task<T> FindAsync(Guid id);
        Task<T> GetAsync(Guid id);
        Task<List<T>> GetManyAsync(IEnumerable<Guid> idList);
        //Task<T> CreateAsync(string name, Guid? parentId, long sorting, string description);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(Guid id, string name, Guid? parentId);
        Task DeleteAsync(Guid id);
        /// <summary>
        /// 获取子目录
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        Task<List<T>> GetChildsAsync(Guid? entityId);
    }
}
```

#### TreeManager

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IczpNet.AbpTrees.Statics;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace IczpNet.AbpTrees
{
    //public class TreeManager<T, TOutput, TWithChildsOuput, TWithParentOuput> : TreeManager<T, TOutput, TWithChildsOuput>, ITreeManager<T, TOutput, TWithChildsOuput, TWithParentOuput>
    //    where T : TreeEntity<T>, new()
    //    where TOutput : class, ITreeInfo
    //    where TWithChildsOuput : class, ITreeWithChildsInfo<TWithChildsOuput>
    //    where TWithParentOuput : class, ITreeWithParentInfo<TWithParentOuput>
    //{
    //    public TreeManager(IRepository<T, Guid> repository) : base(repository) { }

    //    public async Task<TWithParentOuput> GetWithParentAsync(Guid id)
    //    {
    //        var entity = await GetAsync(id);
    //        return ObjectMapper.Map<T, TWithParentOuput>(entity);
    //    }
    //}
    public class TreeManager<T, TOutput, TWithChildsOuput> : TreeManager<T, TOutput>, ITreeManager<T, TOutput, TWithChildsOuput>
        where T : TreeEntity<T>
        where TOutput : class, ITreeInfo
        where TWithChildsOuput : class, ITreeWithChildsInfo<TWithChildsOuput>
    {
        public TreeManager(IRepository<T, Guid> repository) : base(repository) { }

        public override Task RemoveCacheAsync()
        {
            return Cache.RemoveAsync(CacheKey);
        }

        public virtual async Task<List<TWithChildsOuput>> GetAllListWithChildsAsync(Guid? parentId, bool isImportAllChilds = false)
        {
            var allList = await GeAllListByCacheAsync();

            return await GetChildsAsync(allList, parentId, isImportAllChilds);
        }

        private async Task<List<TWithChildsOuput>> GetChildsAsync(List<TOutput> allList, Guid? parentId, bool isImportAllChilds)
        {
            var list = new List<TWithChildsOuput>();

            foreach (var treeInfo in allList.Where(x => x.ParentId == parentId).ToList())
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

        public virtual async Task<List<TWithChildsOuput>> GetRootListAsync(List<Guid> idList)
        {
            var rootList = (await Repository.GetQueryableAsync())
               .Where(x => x.ParentId == null)
               .WhereIf(idList != null && idList.Any(), x => idList.Contains(x.Id))
               .ToList();

            return ObjectMapper.Map<List<T>, List<TWithChildsOuput>>(rootList);
        }
    }

    public class TreeManager<T, TOutput> : TreeManager<T>, ITreeManager<T, TOutput>
        where T : TreeEntity<T>
        where TOutput : class, ITreeInfo
    {
        
        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
        protected IDistributedCache<List<TOutput>> Cache => LazyServiceProvider.LazyGetRequiredService<IDistributedCache<List<TOutput>>>();

        public TreeManager(IRepository<T, Guid> repository) : base(repository) { }

        public override Task RemoveCacheAsync()
        {
            return Cache.RemoveAsync(CacheKey);
        }

        public virtual Task<List<TOutput>> GeAllListByCacheAsync()
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


    public class TreeManager<T> : DomainService, ITreeManager<T>
        where T : TreeEntity<T>
    {
        public virtual string CacheKey => typeof(T).FullName;
        public IRepository<T, Guid> Repository { get; }
        public TreeManager(IRepository<T, Guid> repository)
        {
            Repository = repository;
        }

        public virtual async Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(IEnumerable<Guid> departmentIdList)
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

        public virtual Task<IQueryable<T>> QueryCurrentAndAllChildsAsync(Guid departmentId)
        {
            return QueryCurrentAndAllChildsAsync(new List<Guid>() { departmentId });
        }

        public virtual Task RemoveCacheAsync()
        {
            //return Cache.RemoveAsync(CacheKey);
            return Task.CompletedTask;
        }

        public virtual Task<T> FindAsync(Guid id)
        {
            return Repository.FindAsync(id);
        }

        public virtual Task<T> GetAsync(Guid id)
        {
            return Repository.GetAsync(id);
        }

        public virtual Task<List<T>> GetManyAsync(IEnumerable<Guid> idList)
        {
            return Repository.GetListAsync(x => idList.Contains(x.Id));
        }

        public virtual async Task<T> CreateAsync(T inputEntity)
        {
            Assert.If(await Repository.CountAsync(x => x.Name == inputEntity.Name) > 0, $"Already exists:{inputEntity.Name}");

            //inputEntity.SetId(GuidGenerator.Create());

            //inputEntity.SetName((string)inputEntity.Name);

            //inputEntity.SetFullPath((string)null);

            //inputEntity.SetFullPathName((string)null);

            //inputEntity.SetFullPathPinyin((string)null);

            var entity = await Repository.InsertAsync(inputEntity, autoSave: true);

            if (inputEntity.ParentId.HasValue)
            {
                var parent = await Repository.GetAsync((Guid)inputEntity.ParentId.Value);

                Assert.NotNull((T)parent, $"No such parent entity:{inputEntity.ParentId}");

                entity.SetParent((T)parent);
            }
            else
            {
                entity.SetParent((T)null);
            }

            await RemoveCacheAsync();

            return entity;
        }

        public virtual async Task<T> UpdateAsync(Guid id, string name, Guid? parentId)
        {
            Assert.If(await Repository.CountAsync(x => x.Name == name && x.Id != id) > 0, $"{name} 已经存在");

            var entity = await Repository.FindAsync(id);

            Assert.NotNull(entity, $"目录不存在");

            entity.SetName(name);

            if (parentId.HasValue)
            {
                //变更上级
                var parent = await Repository.GetAsync(parentId.Value);

                Assert.NotNull(parent, $"上级目录不存在");

                entity.SetParent(parent);
            }
            else
            {
                entity.SetParent(null);
            }

            //变更子集
            await ChangeChildsAsync(entity);

            await RemoveCacheAsync();

            return entity;
        }

        protected virtual async Task ChangeChildsAsync(T department)
        {
            foreach (var dep in department.Childs)
            {
                dep.SetParent(department);

                await ChangeChildsAsync(dep);
            }
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id);

            var childCount = entity.Childs.Count();

            Assert.If(childCount > 0, $"有 {childCount} 个子目录,不能删除");

            await Repository.DeleteAsync(entity);

            await RemoveCacheAsync();
        }

        public async Task<List<T>> GetChildsAsync(Guid? entityId)
        {
            //return await Repository.GetListAsync(x => x.ParentId == departmentId);
            return (await Repository.GetQueryableAsync())
                .Where(x => x.ParentId == entityId)
                .OrderByDescending(x => x.Sorting)
                .ToList();
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

namespace IczpNet.AbpTrees
{
    public interface ITreeAppService<
        TTreeInfo,
        TTreeWithChildsDto,
        TTreeWithParentDto>
        :
        ITreeAppService<TTreeInfo, TTreeWithChildsDto>
        where TTreeInfo : ITreeInfo
        where TTreeWithChildsDto : ITreeWithChildsInfo<TTreeWithChildsDto>
        where TTreeWithParentDto : ITreeWithParentInfo<TTreeWithParentDto>
    {
        Task<TTreeWithParentDto> GetWithParentAsync(Guid id);
    }

    public interface ITreeAppService<
        TTreeInfo,
        TTreeWithChildsDto>
        :
        ITreeAppService<TTreeInfo>
        where TTreeInfo : ITreeInfo
        where TTreeWithChildsDto : ITreeWithChildsInfo<TTreeWithChildsDto>
    {
        Task<List<TTreeWithChildsDto>> GetAllListWithChildsAsync(Guid? ParentId, bool IsImportAllChilds);
        Task<List<TTreeWithChildsDto>> GetRootListAsync(List<Guid> idList);
    }

    public interface ITreeAppService<TTreeInfo> : ITreeAppService
        where TTreeInfo : ITreeInfo
    {
        Task<List<TTreeInfo>> GeAllListByCacheAsync();
    }

    public interface ITreeAppService
    {

    }
}

```

### Dtos

#### ITreeGetListInput

```C#
using System;

namespace IczpNet.AbpTrees
{
    public interface ITreeGetListInput
    {
        bool IsEnabledParentId { get; set; }

        int? Depth { get; set; }

        Guid? ParentId { get; set; }

        string Keyword { get; set; }
    }
}

```

#### ITreeInput

```C#
using System;

namespace IczpNet.AbpTrees.Dtos
{
    public interface ITreeInput
    {
        string Name { get; set; }
        Guid? ParentId { get; set; }
    }
}
```

#### TreeGetListInput

```C#
using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTrees.Dtos
{
    public class TreeGetListInput : PagedAndSortedResultRequestDto, ITreeGetListInput
    {
        [DefaultValue(false)]
        public virtual bool IsEnabledParentId { get; set; }

        [DefaultValue(null)]
        public virtual int? Depth { get; set; }

        [DefaultValue(null)]
        public virtual Guid? ParentId { get; set; }

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
    public abstract class TreeAppService<
        TEntity, 
        TGetOutputDto, 
        TGetListOutputDto, 
        TGetListInput, 
        TCreateInput, 
        TUpdateInput, 
        TTreeInfo, 
        TTreeWithChildsDto, 
        TTreeWithParentDto>
        : 
        TreeAppService<
            TEntity, 
            TGetOutputDto, 
            TGetListOutputDto, 
            TGetListInput, 
            TCreateInput, 
            TUpdateInput, 
            TTreeInfo, 
            TTreeWithChildsDto>
        , 
        ITreeAppService<
        TTreeInfo, 
        TTreeWithChildsDto, 
        TTreeWithParentDto>
        where TEntity : class, ITreeEntity<TEntity>, ITreeEntity
        where TGetOutputDto : IEntityDto<Guid>
        where TGetListOutputDto : IEntityDto<Guid>
        where TGetListInput : ITreeGetListInput
        where TCreateInput : ITreeInput
        where TUpdateInput : ITreeInput
        where TTreeInfo : ITreeInfo
        where TTreeWithChildsDto : ITreeWithChildsInfo<TTreeWithChildsDto>
        where TTreeWithParentDto : ITreeWithParentInfo<TTreeWithParentDto>
    {
        //protected override ITreeManager<TEntity, TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto> TreeManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto>>();
        protected TreeAppService(IRepository<TEntity, Guid> repository) : base(repository) { }

        [HttpGet]
        public virtual async Task<TTreeWithParentDto> GetWithParentAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var entity = await base.GetEntityByIdAsync(id);

            return ObjectMapper.Map<TEntity, TTreeWithParentDto>(entity);
        }
    }
    public abstract class TreeAppService<
        TEntity, 
        TGetOutputDto, 
        TGetListOutputDto, 
        TGetListInput, 
        TCreateInput, 
        TUpdateInput, 
        TTreeInfo, 
        TTreeWithChildsDto>
        : 
        TreeAppService<
            TEntity, 
            TGetOutputDto, 
            TGetListOutputDto, 
            TGetListInput, 
            TCreateInput, 
            TUpdateInput, 
            TTreeInfo>
        , 
        ITreeAppService<
        TTreeInfo, 
        TTreeWithChildsDto>
        where TEntity : class, ITreeEntity<TEntity>, ITreeEntity
        where TGetOutputDto : IEntityDto<Guid>
        where TGetListOutputDto : IEntityDto<Guid>
        where TGetListInput : ITreeGetListInput
        where TCreateInput : ITreeInput
        where TUpdateInput : ITreeInput
        where TTreeInfo : ITreeInfo
        where TTreeWithChildsDto : ITreeWithChildsInfo<TTreeWithChildsDto>
    {
        protected  ITreeManager<TEntity, TTreeInfo, TTreeWithChildsDto> TreeWithChildsManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TTreeInfo, TTreeWithChildsDto>>();
        protected TreeAppService(IRepository<TEntity, Guid> repository) : base(repository) { }

        [HttpGet]
        public virtual async Task<List<TTreeWithChildsDto>> GetAllListWithChildsAsync(Guid? ParentId, bool IsImportAllChilds)
        {
            await CheckGetListPolicyAsync();

            return await TreeWithChildsManager.GetAllListWithChildsAsync(ParentId, IsImportAllChilds);
        }
        [HttpGet]
        public virtual async Task<List<TTreeWithChildsDto>> GetRootListAsync(List<Guid> idList)
        {
            await CheckGetPolicyAsync();

            return await TreeWithChildsManager.GetRootListAsync(idList);
        }
    }
    public abstract class TreeAppService<
        TEntity, 
        TGetOutputDto, 
        TGetListOutputDto, 
        TGetListInput, 
        TCreateInput, 
        TUpdateInput, 
        TTreeInfo>
        : 
        TreeAppService<
            TEntity, 
            TGetOutputDto, 
            TGetListOutputDto, 
            TGetListInput, 
            TCreateInput, 
            TUpdateInput>
        , 
        ITreeAppService<TTreeInfo>
        where TEntity : class, ITreeEntity<TEntity>, ITreeEntity
        where TGetOutputDto : IEntityDto<Guid>
        where TGetListOutputDto : IEntityDto<Guid>
        where TGetListInput : ITreeGetListInput
        where TCreateInput : ITreeInput
        where TUpdateInput : ITreeInput
        where TTreeInfo : ITreeInfo
    {
        protected  ITreeManager<TEntity, TTreeInfo> TreeCacheManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TTreeInfo>>();
        protected TreeAppService(IRepository<TEntity, Guid> repository) : base(repository) { }


        [HttpGet]
        public virtual async Task<List<TTreeInfo>> GeAllListByCacheAsync()
        {
            await CheckGetListPolicyAsync();

            return await TreeCacheManager.GeAllListByCacheAsync();
        }
    }


    public abstract class TreeAppService<
        TEntity, 
        TGetOutputDto, 
        TGetListOutputDto, 
        TGetListInput, 
        TCreateInput, 
        TUpdateInput>
        : 
        CrudAppService<
            TEntity, 
            TGetOutputDto, 
            TGetListOutputDto, 
            Guid, 
            TGetListInput, 
            TCreateInput, 
            TUpdateInput>
        //:ITreeAppService<TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto>
    where TEntity : class, ITreeEntity<TEntity>, ITreeEntity
    where TGetOutputDto : IEntityDto<Guid>
    where TGetListOutputDto : IEntityDto<Guid>
    where TGetListInput : ITreeGetListInput
    where TCreateInput : ITreeInput
    where TUpdateInput : ITreeInput
    {
        protected virtual ITreeManager<TEntity> TreeManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity>>();

        public TreeAppService(IRepository<TEntity, Guid> repository) : base(repository) { }

        protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
        {
            return query.OrderByDescending(x => x.Sorting);
        }

        [HttpGet]
        public override Task<TGetOutputDto> GetAsync(Guid id)
        {
            return base.GetAsync(id);
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
                .WhereIf(input.Depth.HasValue, x => x.Depth == input.Depth)
                .WhereIf(input.IsEnabledParentId, x => x.ParentId == input.ParentId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword))
               ;
        }


        [HttpPost]
        public override async Task<TGetOutputDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePolicyAsync();

            var inputEntity = MapToEntity(input);

            inputEntity.FillCreate(GuidGenerator.Create(), input.Name, input.ParentId);

            var entity = await TreeManager.CreateAsync(inputEntity);

            return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
        }

        [HttpPost]
        public override async Task<TGetOutputDto> UpdateAsync(Guid id, TUpdateInput input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await TreeManager.UpdateAsync(id, input.Name, input.ParentId);

            await MapToEntityAsync(input, entity);

            return await MapToGetOutputDtoAsync(entity);
        }

        [HttpPost]
        public override async Task DeleteAsync(Guid id)
        {
            await CheckDeletePolicyAsync();

            await TreeManager.DeleteAsync(id);
        }
    }
}

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
        public virtual double Sorting { get; set; }
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
using IczpNet.AbpTrees.Dtos;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentGetListInput
/// </summary>
public class DepartmentGetListInput : TreeGetListInput, ITreeGetListInput
{

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
    public virtual double Sorting { get; set; }
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
    public virtual double Sorting { get; set; }
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
using IczpNet.AbpTreesDemo.Departments.Dtos;
using System;
using Volo.Abp.Application.Services;

namespace IczpNet.AbpTreesDemo.Departments
{
    public interface IDepartmentAppSevice :
        ICrudAppService<
            DepartmentDto,
            DepartmentDto,
            Guid,
            DepartmentGetListInput,
            DepartmentCreateInput,
            DepartmentUpdateInput>
        , ITreeAppService<
            DepartmentInfo,
            DepartmentWithChildsDto,
            DepartmentWithParentDto>
    {
    }
}


```

### Application CRUD

`IczpNet.AbpTreesDemo.Application`  > `DepartmentAppsevice.cs`

```C#
using IczpNet.AbpTrees;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using System;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTreesDemo.Departments
{
    public class DepartmentAppService 
        : TreeAppService<
            Department, 
            DepartmentDto, 
            DepartmentDto, 
            DepartmentGetListInput, 
            DepartmentCreateInput, 
            DepartmentUpdateInput, 
            DepartmentInfo, 
            DepartmentWithChildsDto, 
            DepartmentWithParentDto>, 
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

   

   