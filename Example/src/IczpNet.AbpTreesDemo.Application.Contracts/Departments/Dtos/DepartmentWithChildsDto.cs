using IczpNet.AbpTrees;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentWithChildsDto
/// </summary>
public class DepartmentWithChildsDto : TreeWithChildsInfo<DepartmentWithChildsDto>
{
    public virtual int ChildsCount { get; set; }

    //public virtual int UserCount { get; set; }

    //public virtual List<DepartmentWithChildsDto> Childs { get; set; }
}
