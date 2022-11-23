using AutoMapper;
using IczpNet.AbpTrees;
using System;

namespace IczpNet.AbpTreesDemo;

public class AbpTreesDomainAutoMapperProfile : Profile
{
    public AbpTreesDomainAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        /* ResolveUsing
         * The ResolveUsing method consolidated with MapFrom:
         * //https://docs.automapper.org/en/latest/8.0-Upgrade-Guide.html
         */

        //CreateMap(typeof(TreeEntity<>), typeof(TreeInfo)).ConvertUsing(typeof(Converter<,>));
        CreateMap(typeof(TreeEntity<,>), typeof(TreeWithChildsInfo<,>)).ConvertUsing(typeof(Converter<,>));
        CreateMap(typeof(TreeEntity<,>), typeof(TreeWithParentInfo<,>)).ConvertUsing(typeof(Converter<,>));
    }
}
