using IczpNet.AbpTreesDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace IczpNet.AbpTreesDemo.Permissions;

public class AbpTreesDemoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AbpTreesDemoPermissions.GroupName, L("Permission:AbpTreesDemo"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpTreesDemoResource>(name);
    }
}
