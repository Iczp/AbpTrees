﻿using IczpNet.AbpTrees.Dtos;
using System;

namespace IczpNet.AbpTreesDemo.Departments.Dtos;

/// <summary>
/// DepartmentUpdateInput
/// </summary>
public class DepartmentUpdateInput : ITreeInput
{

    /// <summary>
    /// 上级部门
    /// </summary>
    public virtual Guid? ParentId { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; set; }
    /// <summary>
    /// 排序（越大越前面） DESC
    /// </summary>
    public virtual double Sorting { get; set; }
    /// <summary>
    /// 说明
    /// </summary>
    public virtual string Description { get; set; }

}
