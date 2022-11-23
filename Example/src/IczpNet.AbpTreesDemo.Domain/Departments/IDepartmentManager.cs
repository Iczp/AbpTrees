using IczpNet.AbpTrees;
using System;

namespace IczpNet.AbpTreesDemo.Departments
{
    public interface IDepartmentManager : ITreeManager<Department, Guid>
    {
    }
}
