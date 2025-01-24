using System.Diagnostics;
using Volo.Abp;

namespace IczpNet.AbpTrees.Statics;

[DebuggerStepThrough]
public static class Assert
{
    public static void If(bool value, string message)
    {
        if (value)
        {
            throw new UserFriendlyException(message);
        }
    }
    public static T NotNull<T>(T value, string message)
    {
        if (value == null)
        {
            throw new UserFriendlyException(message);
        }

        return value;
    }
}
