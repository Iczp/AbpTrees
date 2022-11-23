using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IczpNet.AbpTrees
{
    public interface ITreeAppService<TKey, TTreeInfo> : ITreeAppService<TKey>
        where TKey : struct
        where TTreeInfo : ITreeInfo<TKey>
    {
        Task<List<TTreeInfo>> GetAllByCacheAsync();
    }

    public interface ITreeAppService<TKey> where TKey : struct
    {
        Task<DateTime> RepairDataAsync();
    }
}
