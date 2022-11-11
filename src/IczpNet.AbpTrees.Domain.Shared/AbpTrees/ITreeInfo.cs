using System;


namespace IczpNet.AbpTrees
{
    /// <summary>
    /// TreeInfo
    /// </summary>
    public interface ITreeInfo 
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        Guid? ParentId { get; set; }
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
