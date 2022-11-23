using IczpNet.AbpTrees;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentWithChildsDto : TreeWithChildsInfo<DepartmentWithChildsDto, Guid>
{
    public virtual int ChildsCount { get; set; }
}
