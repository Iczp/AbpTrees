namespace IczpNet.AbpTrees
{
    public interface ITreeWithParentInfo<T> : ITreeInfo
    {
        T Parent { get; set; }
    }
}
