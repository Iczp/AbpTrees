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

            foreach (var treeInfo in allList.Where(x => x.ParentId.Equals(parentId)).ToList())
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
        protected IDistributedCache<TOutput> ItemCache => LazyServiceProvider.LazyGetRequiredService<IDistributedCache<TOutput>>();
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

        public virtual Task<TOutput> GetItemByCacheAsync(TKey id)
        {
            return ItemCache.GetOrAddAsync(id.ToString(), async () =>
            {
                var entity = await GetAsync(id);

                return await MapToOuputAsync(entity);
            });
        }

        protected virtual Task<TOutput> MapToOuputAsync(T entity)
        {
            return Task.FromResult(ObjectMapper.Map<T, TOutput>(entity));
        }

        public virtual async Task<List<TOutput>> GetManyByCacheAsync(List<TKey> idList)
        {
            var list = new List<TOutput>();

            foreach (var id in idList)
            {
                var entity = await GetItemByCacheAsync(id);

                if (entity != null)
                {
                    list.Add(entity);
                }
                else
                {
                    Logger.LogWarning($"[{nameof(GetManyByCacheAsync)}] No such entity[{typeof(TOutput)}] by id:${id}");
                }
            }
            return list;
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

        public async Task<List<T>> GetManyAsync(List<TKey> idList)
        {
            var list = new List<T>();

            foreach (var id in idList)
            {
                list.Add(await GetAsync(id));
            }
            return list;
        }

        public virtual async Task<List<T>> GetManyAsync(IEnumerable<TKey> idList)
        {
            var list = new List<T>();

            foreach (var id in idList)
            {
                list.Add(await GetAsync(id));
            }
            return list;
        }

        public virtual async Task<T> CreateAsync(T inputEntity, bool isUnique = true)
        {
            Assert.If(isUnique && await Repository.CountAsync(x => x.Name == inputEntity.Name) > 0, $"Already exists name:{inputEntity.Name}");

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

            Assert.If(entity.Name.Contains(AbpTreesConsts.SplitPath), $"[Name] cannot contains char:\"{AbpTreesConsts.SplitPath}\"");

            Assert.NotNull(entity.ParentId.Equals(entity.Id), $"ParentId[{entity.ParentId}] may cause infinite loop.");

            //var arr = entity.FullPath.Split(AbpTreesConsts.SplitPath);

            //Array.IndexOf(arr, entity.ParentId);

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