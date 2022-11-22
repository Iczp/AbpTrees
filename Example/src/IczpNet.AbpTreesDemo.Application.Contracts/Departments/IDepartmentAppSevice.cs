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
        , ITreeAppService<DepartmentInfo>
    {
    }
}
