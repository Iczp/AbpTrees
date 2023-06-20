using AutoMapper;

namespace IczpNet.AbpTrees;

public class AbpTreesApplicationAutoMapperProfile : Profile
{
    public AbpTreesApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap(typeof(TreeEntity<,>), typeof(TreeInfo<>));
    }
}
