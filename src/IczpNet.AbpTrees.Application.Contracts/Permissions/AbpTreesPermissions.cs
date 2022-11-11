using Volo.Abp.Reflection;

namespace IczpNet.AbpTrees.Permissions;

public class AbpTreesPermissions
{
    public const string GroupName = "AbpTrees";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AbpTreesPermissions));
    }
}
