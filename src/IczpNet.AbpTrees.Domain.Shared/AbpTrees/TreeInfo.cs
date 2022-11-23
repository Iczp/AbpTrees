namespace IczpNet.AbpTrees
{
    /// <summary>
    /// TreeInfo
    /// </summary>
    public class TreeInfo<TKey> : ITreeInfo<TKey> where TKey : struct
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual TKey Id { get; set; }
        /// <summary>
        /// ParentId
        /// </summary>
        public virtual TKey? ParentId { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public virtual int Depth { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public virtual string FullPath { get; set; }
        /// <summary>
        /// 路径名称
        /// </summary>
        public virtual string FullPathName { get; set; }
    }
}
