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
        Task<List<TTreeInfo>> GetAllListByCacheAsync();
    }

    public interface ITreeAppService
    {

    }
}
