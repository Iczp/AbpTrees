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
        where T : TreeEntity<T>, new()
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
        where T : TreeEntity<T>, new()
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
        where T : TreeEntity<T>, new()
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