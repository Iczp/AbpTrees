using IczpNet.AbpTrees;
using System;
using Volo.Abp.Application.Services;

namespace IczpNet.AbpTreesDemo.Departments
{
    public interface IDepartmentAppSevice<TGetOutputDto, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput, TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto>
        : ICrudAppService<TGetOutputDto, TGetListOutputDto, Guid, TGetListInput, TCreateInput, TUpdateInput>
        , ITreeAppService<TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto>
         where TTreeInfo : ITreeInfo
        where TTreeWithChildsDto : ITreeWithChildsInfo<TTreeWithChildsDto>
        where TTreeWithParentDto : ITreeWithParentInfo<TTreeWithParentDto>
    {
    }
}
