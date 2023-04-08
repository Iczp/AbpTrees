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

        Task<TTreeOutput> GetItemByCacheAsync(TKey id);

        Task<List<TTreeOutput>> GetManyByCacheAsync(List<TKey> idList);
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
        Task<T> CreateAsync(T entity, bool isUnique = true);
        Task<T> UpdateAsync(T entity, bool isUnique = true);
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