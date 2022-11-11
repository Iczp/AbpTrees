using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace IczpNet.AbpTrees.Dtos
{
    public class TreeGetListInput: ITreeGetListInput
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public virtual bool IsEnabledParentId { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [DefaultValue(null)]
        public virtual int? Depth { get; set; }
        /// <summary>
        /// 上级部门
        /// </summary>
        [DefaultValue(null)]
        public virtual Guid? ParentId { get; set; }
        /// <summary>
        /// 关键字(支持拼音)
        /// </summary>
        [DefaultValue(null)]
        public virtual string Keyword { get; set; }
    }
}
