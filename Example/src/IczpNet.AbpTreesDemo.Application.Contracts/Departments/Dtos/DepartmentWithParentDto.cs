using IczpNet.AbpTrees;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentWithParentDto : TreeWithParentInfo<DepartmentWithParentDto, Guid>
{
    public virtual double Sorting { get; set; }
    public virtual string Description { get; set; }
}
