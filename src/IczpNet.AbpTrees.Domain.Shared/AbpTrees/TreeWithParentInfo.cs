namespace IczpNet.AbpTrees;

public class TreeWithParentInfo<T, TKey> : TreeInfo<TKey>, ITreeWithParentInfo<T>
    where TKey : struct
{
    public virtual T Parent { get; set; }

}
