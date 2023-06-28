using IczpNet.AbpTrees.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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

        Task<PagedResultDto<TTreeInfo>> GetAllByCacheAsync(TreeGetListInput<TKey> input);
    }

    public interface ITreeAppService<TGetOutputDto, TGetListOutputDto, TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TKey : struct
    {

        Task<List<TGetOutputDto>> GetManyAsync(List<TKey> idList);

        Task DeleteManyAsync(List<TKey> idList);

        Task<string> RepairDataAsync(int maxResultCount = 100, int skinCount = 0);


    }
}
