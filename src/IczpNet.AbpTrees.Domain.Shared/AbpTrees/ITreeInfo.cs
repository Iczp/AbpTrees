namespace IczpNet.AbpTrees
{
    public interface ITreeInfo<TKey> where TKey : struct
    {
        /// <summary>
        /// Id
        /// </summary>
        TKey Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        TKey? ParentId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        int Depth { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        string FullPath { get; set; }
        /// <summary>
        /// 路径名称
        /// </summary>
        string FullPathName { get; set; }
    }
}
