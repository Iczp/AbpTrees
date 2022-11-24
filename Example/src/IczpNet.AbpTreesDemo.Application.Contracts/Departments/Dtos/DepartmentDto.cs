using System;
using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTreesDemo.Departments.Dtos
{
    public class DepartmentDto : DepartmentInfo, IEntityDto<Guid>
    {
        public virtual double Sorting { get; set; }
        public virtual string Description { get; set; }
    }
}
