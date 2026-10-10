using IczpNet.AbpTreesDemo.Departments;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace IczpNet.AbpTreesDemo;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DepartmentToDepartmentDtoMapper : MapperBase<Department, DepartmentDto>
{
    public override partial DepartmentDto Map(Department source);
    public override partial void Map(Department source, DepartmentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DepartmentToDepartmentWithParentDtoMapper : MapperBase<Department, DepartmentWithParentDto>
{
    public override partial DepartmentWithParentDto Map(Department source);
    public override partial void Map(Department source, DepartmentWithParentDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DepartmentToDepartmentWithChildsDtoMapper : MapperBase<Department, DepartmentWithChildsDto>
{
    [UserMapping(Default = true)]
    public override DepartmentWithChildsDto Map(Department source)
    {
        var destination = MapCore(source);
        destination.ChildsCount = source.GetChildsCount();
        return destination;
    }

    public override void Map(Department source, DepartmentWithChildsDto destination)
    {
        MapCore(source, destination);
        destination.ChildsCount = source.GetChildsCount();
    }

    [MapperIgnoreTarget(nameof(DepartmentWithChildsDto.ChildsCount))]
    private partial DepartmentWithChildsDto MapCore(Department source);

    [MapperIgnoreTarget(nameof(DepartmentWithChildsDto.ChildsCount))]
    private partial void MapCore(Department source, DepartmentWithChildsDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class DepartmentCreateInputToDepartmentMapper : MapperBase<DepartmentCreateInput, Department>
{
    public override partial Department Map(DepartmentCreateInput source);
    public override partial void Map(DepartmentCreateInput source, Department destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class DepartmentUpdateInputToDepartmentMapper : MapperBase<DepartmentUpdateInput, Department>
{
    public override partial Department Map(DepartmentUpdateInput source);
    public override partial void Map(DepartmentUpdateInput source, Department destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DepartmentToDepartmentInfoMapper : MapperBase<Department, DepartmentInfo>
{
    public override partial DepartmentInfo Map(Department source);
    public override partial void Map(Department source, DepartmentInfo destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DepartmentInfoToDepartmentWithChildsDtoMapper : MapperBase<DepartmentInfo, DepartmentWithChildsDto>
{
    [MapperIgnoreTarget(nameof(DepartmentWithChildsDto.ChildsCount))]
    [MapperIgnoreTarget(nameof(DepartmentWithChildsDto.Childs))]
    public override partial DepartmentWithChildsDto Map(DepartmentInfo source);

    [MapperIgnoreTarget(nameof(DepartmentWithChildsDto.ChildsCount))]
    [MapperIgnoreTarget(nameof(DepartmentWithChildsDto.Childs))]
    public override partial void Map(DepartmentInfo source, DepartmentWithChildsDto destination);
}
