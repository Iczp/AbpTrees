using AutoMapper;
using IczpNet.AbpTreesDemo.Departments;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using Volo.Abp.AutoMapper;

namespace IczpNet.AbpTreesDemo;

public class AbpTreesDemoApplicationAutoMapperProfile : Profile
{
    public AbpTreesDemoApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Department, DepartmentDto>(MemberList.Destination);
        CreateMap<Department, DepartmentWithParentDto>(MemberList.Destination);
        CreateMap<Department, DepartmentWithChildsDto>(MemberList.Destination)
            // MaxDepth
            //.MaxDepth(depth: 3)
            .ForMember(s => s.ChildsCount, map => map.MapFrom(d => d.GetChildsCount()))
             //.ForMember(s => s.UserCount, map => map.MapFrom(d => d.GetUserCount()))
             ;
        CreateMap<DepartmentCreateInput, Department>(MemberList.Source).IgnoreAllPropertiesWithAnInaccessibleSetter();
        CreateMap<DepartmentUpdateInput, Department>(MemberList.Source).IgnoreAllPropertiesWithAnInaccessibleSetter();


        CreateMap<Department, DepartmentInfo>();
        CreateMap<DepartmentInfo, DepartmentWithChildsDto>()
            .Ignore(x => x.ChildsCount)
            .Ignore(x => x.Childs);
    }
}
