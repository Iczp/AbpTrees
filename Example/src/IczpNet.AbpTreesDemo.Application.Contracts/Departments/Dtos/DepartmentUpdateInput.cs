using IczpNet.AbpTrees.Dtos;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentUpdateInput : ITreeInput<Guid>
{
    public virtual Guid? ParentId { get; set; }
    public virtual string Name { get; set; }
    public virtual double Sorting { get; set; }
    public virtual string Description { get; set; }

}
