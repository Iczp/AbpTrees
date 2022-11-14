using IczpNet.AbpTrees;
using System;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTreesDemo.Departments
{
    public class DepartmentManager : TreeManager<Department>, IDepartmentManager
    {
        public DepartmentManager(IRepository<Department, Guid> repository) : base(repository)
        {
        }
    }
}
