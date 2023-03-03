using IczpNet.AbpTrees.Statics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace IczpNet.AbpTrees
{
    public abstract class TreeEntity<T, TKey> : FullAuditedAggregateRoot<TKey>, ITreeEntity<T, TKey>
        where T : ITreeEntity<TKey>
        where TKey : struct
    {
        public virtual string GetSplitString() => AbpTreesConsts.SplitPath;

        [MaxLength(64)]
        [Required(ErrorMessage = "Name Required.")]
        public virtual string Name { get; protected set; }

        public virtual TKey? ParentId { get; set; }

        [MaxLength(1000)]
        [Required]
        public virtual string FullPath { get; protected set; }

        [MaxLength(1000)]
        [Required]
        public virtual string FullPathName { get; protected set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Range(0, 1024)]
        public virtual int Depth { get; protected set; }

        public virtual double Sorting { get; set; }

        [MaxLength(500)]
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
        [InverseProperty(nameof(Parent))]
        public virtual IEnumerable<T> Childs { get; protected set; }

        protected TreeEntity()
        {

        }

        protected TreeEntity(TKey id, string name, TKey? parentId) : base(id)
        {
            SetId(id);
            SetParentId(parentId);
            SetName(name);
            SetFullPath(null);
            SetFullPathName(null);
        }

        protected TreeEntity(string name, TKey? parentId)
        {
            SetParentId(parentId);
            SetName(name);
            SetFullPath(null);
            SetFullPathName(null);
        }

        public virtual void SetParentId(TKey? parentId)
        {
            ParentId = parentId;
        }

        protected virtual void SetId(TKey id)
        {
            Id = id;
        }

        public virtual void SetName(string name)
        {
            Name = name;
        }

        protected virtual void SetFullPath(string parentPath)
        {
            FullPath = parentPath.IsNullOrEmpty() ? $"{Id}" : $"{parentPath}{GetSplitString()}{Id}";
        }

        protected virtual void SetFullPathName(string parentPathName)
        {
            FullPathName = parentPathName.IsNullOrEmpty() ? $"{Name}" : $"{parentPathName}{GetSplitString()}{Name}";
        }

        protected virtual void SetDepth(int depth)
        {
            Depth = depth;
        }

        public virtual void SetParent(T parent)
        {
            if (parent == null)
            {
                SetDepth(0);
                SetFullPath(null);
                SetFullPathName(null);
            }
            else
            {
                Parent = parent;
                Assert.If(Parent.Depth >= AbpTreesConsts.MaxDepth, $"超出最大层级:{AbpTreesConsts.MaxDepth}");
                SetDepth(Parent.Depth + 1);
                SetFullPath(parent.FullPath);
                SetFullPathName(parent.FullPathName);
            }
        }

        
    }
}