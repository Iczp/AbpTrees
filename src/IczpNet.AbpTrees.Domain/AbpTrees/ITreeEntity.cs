using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace IczpNet.AbpTrees
{
    public interface ITreeEntity<T> : ITreeEntity where T : ITreeEntity
    {

        T Parent { get; }
        IEnumerable<T> Childs { get; }
        void FillCreate(Guid id,string name, Guid? parentId);
        //void SetParent(T parent);
        //void SetId(Guid id);
    }


    public interface ITreeEntity : IEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        ///// <summary>
        ///// 名称_拼音
        ///// </summary>
        //string Name_Pinyin { get; }

        ///// <summary>
        ///// 名称_拼音
        ///// </summary>
        //string Name_PY { get; }

        /// <summary>
        /// 父级Id
        /// </summary>
        Guid? ParentId { get; }

        /// <summary>
        /// 全路径
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// 全路径名称
        /// </summary>
        string FullPathName { get; }

        ///// <summary>
        ///// 全路径拼音
        ///// </summary>
        //string FullPathPinyin { get; }

        /// <summary>
        /// 层级
        /// </summary>
        int Depth { get; }

        /// <summary>
        /// 排序（越大越前面） DESC
        /// </summary>
        double Sorting { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        string Description { get; set; }
    }
}
