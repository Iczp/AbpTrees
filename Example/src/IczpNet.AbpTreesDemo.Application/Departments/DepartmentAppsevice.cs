using IczpNet.AbpTrees;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTreesDemo.Departments
{

    [Route($"Api/App/{AbpTreesDemoRemoteServiceConsts.ModuleName}/[Controller]/[Action]")]
    public class DepartmentAppService
        : TreeAppService<
            Department,
            Guid,
            DepartmentDto,
            DepartmentDto,
            DepartmentGetListInput,
            DepartmentCreateInput,
            DepartmentUpdateInput,
            DepartmentInfo>,
        IDepartmentAppSevice
    {
        public DepartmentAppService(IRepository<Department, Guid> repository) : base(repository)
        {
        }
    }
}
