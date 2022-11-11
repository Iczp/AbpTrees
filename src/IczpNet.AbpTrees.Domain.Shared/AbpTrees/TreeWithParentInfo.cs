namespace IczpNet.AbpTrees
{
    /// <summary>
    /// TreeWithChildsInfo
    /// </summary>
    public class TreeWithParentInfo<T> : TreeInfo, ITreeWithParentInfo<T>
    {
        public virtual T Parent { get; set; }

    }
}
