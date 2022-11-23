using IczpNet.AbpTrees;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentWithParentDto : TreeWithParentInfo<DepartmentWithParentDto, Guid>
{
    /// <summary>
    /// 排序（越大越前面） DESC
    /// </summary>
    public virtual double Sorting { get; set; }
    /// <summary>
    /// 说明
    /// </summary>
    public virtual string Description { get; set; }
}
