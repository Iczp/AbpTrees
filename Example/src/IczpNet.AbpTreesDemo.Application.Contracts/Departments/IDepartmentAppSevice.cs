using IczpNet.AbpTrees;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using System;

namespace IczpNet.AbpTreesDemo.Departments
{
    public interface IDepartmentAppSevice :
        ITreeAppService<DepartmentDto,
            DepartmentDto,
            Guid,
            DepartmentGetListInput,
            DepartmentCreateInput,
            DepartmentUpdateInput, DepartmentInfo>
    {
    }
}
