using System;

namespace IczpNet.AbpTrees
{
    public interface ITreeGetListInput
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsEnabledParentId { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        int? Depth { get; set; }
        /// <summary>
        /// 上级部门
        /// </summary>
        Guid? ParentId { get; set; }
        /// <summary>
        /// 关键字(支持拼音)
        /// </summary>
        string Keyword { get; set; }
    }
}
