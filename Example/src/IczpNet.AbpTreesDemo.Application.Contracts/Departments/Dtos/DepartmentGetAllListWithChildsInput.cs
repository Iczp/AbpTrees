using System;
using System.ComponentModel;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

public class DepartmentGetAllListWithChildsInput 
{
    [DefaultValue(null)]
    public virtual Guid? ParentId { get; set; }
    public virtual bool IsImportAllChilds { get; set; }
}
