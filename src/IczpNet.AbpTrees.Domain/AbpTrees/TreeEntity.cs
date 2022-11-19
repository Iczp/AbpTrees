using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using IczpNet.AbpTrees.Statics;
using Volo.Abp.Domain.Entities.Auditing;

namespace IczpNet.AbpTrees
{
    public abstract class TreeEntity<T> : FullAuditedAggregateRoot<Guid>, ITreeEntity<T> where T : ITreeEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(64)]
        [Required(ErrorMessage = "名称不能为NUll")]
        public virtual string Name { get; protected set; }

        ///// <summary>
        ///// 名称_拼音
        ///// </summary>
        //[StringLength(AbpTreeConsts.Name_PinyinMaxLength)]
        //[MaxLength(AbpTreeConsts.Name_PinyinMaxLength)]
        //// [Comment("名称_拼音")]
        //public virtual string Name_Pinyin { get; protected set; }

        ///// <summary>
        ///// 名称_拼音
        ///// </summary>
        //[StringLength(AbpTreeConsts.Name_PYMaxLength)]
        //[MaxLength(AbpTreeConsts.Name_PYMaxLength)]
        //// [Comment("名称_拼音")]
        //public virtual string Name_PY { get; protected set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        // [Comment("父级Id")]
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// 全路径
        /// </summary>
        [MaxLength(1000)]
        [Required]
        // [Comment("全路径")]
        public virtual string FullPath { get; protected set; }

        /// <summary>
        /// 全路径名称
        /// </summary>
        [MaxLength(1000)]
        [Required]
        // [Comment("全路径名称")]
        public virtual string FullPathName { get; protected set; }

        ///// <summary>
        ///// 全路径拼音
        ///// </summary>
        //[MaxLength(1000)]
        //[Required]
        //// [Comment("全路径拼音")]
        //public virtual string FullPathPinyin { get; protected set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Range(0, 16)]
        // [Comment("层级")]
        public virtual int Depth { get; protected set; }

        /// <summary>
        /// 排序（越大越前面） DESC
        /// </summary>
        // [Comment("排序（越大越前面） DESC")]
        public virtual double Sorting { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [MaxLength(500)]
        // [Comment("说明")]
        public virtual string Description { get; set; }

        public virtual int GetChildsCount()
        {
            return Childs.Count();
        }

        /// <summary>
        /// 父级角色
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public virtual T Parent { get; protected set; }

        /// <summary>
        /// 子集合
        /// </summary>
        public virtual IEnumerable<T> Childs { get; protected set; }

        protected TreeEntity()
        {

        }

        protected TreeEntity(Guid id, string name, Guid? parentId) : base(id)
        {
            FillCreate(id, name, parentId);
        }

        public virtual void FillCreate(Guid id, string name, Guid? parentId)
        {
            SetId(id);
            SetParentId(parentId);
            SetName(name);
            SetFullPath(null);
            SetFullPathName(null);
        }

        public void FillUpdate(string name, Guid? parentId)
        {
            SetParentId(parentId);
            SetName(name);
        }
        protected virtual void SetParentId(Guid? parentId)
        {
            ParentId = parentId;
        }

        protected virtual void SetId(Guid id)
        {
            Id = id;
        }
        public virtual void SetName(string name)
        {
            Name = name;
        }

        public virtual void SetFullPath(string parentPath)
        {
            FullPath = parentPath.IsNullOrEmpty() ? $"{Id}" : $"{parentPath}{AbpTreesConsts.SplitPath}{Id}";
        }

        public virtual void SetFullPathName(string parentPathName)
        {
            FullPathName = parentPathName.IsNullOrEmpty() ? $"{Name}" : $"{parentPathName}{AbpTreesConsts.SplitPath}{Name}";
        }

        protected virtual void SetDepth(int depth)
        {
            Depth = depth;
        }


        public virtual void SetParent(T parent)
        {
            if (parent == null)
            {
                SetFullPath(null);
                SetFullPathName(null);
                //SetFullPathPinyin(null);
                return;
            }
            Parent = parent;

            Assert.If(Parent.Depth >= AbpTreesConsts.MaxDepth, $"超出最大层级:{AbpTreesConsts.MaxDepth}");

            SetDepth(Parent.Depth + 1);
            SetFullPath(parent.FullPath);
            SetFullPathName(parent.FullPathName);
        }

        
    }
}