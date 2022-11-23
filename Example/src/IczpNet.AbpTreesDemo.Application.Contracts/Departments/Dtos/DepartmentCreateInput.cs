using IczpNet.AbpTrees.Dtos;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentCreateInput
/// </summary>
public class DepartmentCreateInput : DepartmentUpdateInput, ITreeInput<Guid>
{

}
