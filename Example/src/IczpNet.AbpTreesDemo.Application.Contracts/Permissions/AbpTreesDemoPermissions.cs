using Volo.Abp.Reflection;

namespace IczpNet.AbpTreesDemo.Permissions;

public class AbpTreesDemoPermissions
{
    public const string GroupName = "AbpTreesDemo";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(AbpTreesDemoPermissions));
    }
}
