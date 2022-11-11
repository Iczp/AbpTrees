namespace IczpNet.AbpTrees;

public static class AbpTreesDbProperties
{
    public static string DbTablePrefix { get; set; } = "AbpTrees";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "AbpTrees";
}
