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