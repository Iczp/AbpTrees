using System;
using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTreesDemo.Departments.Dtos
{
    /// <summary>
    /// DepartmentDto
    /// </summary>
    public class DepartmentDto : DepartmentInfo, IEntityDto<Guid>
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
}
