using IczpNet.AbpTrees;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using System;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTreesDemo.Departments
{
    public class DepartmentAppService 
        : TreeAppService<
            Department, 
            DepartmentDto, 
            DepartmentDto, 
            DepartmentGetListInput, 
            DepartmentCreateInput, 
            DepartmentUpdateInput, 
            DepartmentInfo, 
            DepartmentWithChildsDto, 
            DepartmentWithParentDto>, 
        IDepartmentAppSevice
    {
        public DepartmentAppService(IRepository<Department, Guid> repository) : base(repository)
        {
        }
    }
}
