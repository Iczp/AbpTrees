using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace IczpNet.AbpTrees
{
    public interface ITreeEntity<T, TKey> : ITreeEntity<TKey>
        where T : ITreeEntity<TKey>
        where TKey : struct
    {
        T Parent { get; }
        IEnumerable<T> Childs { get; }
        void SetName(string name);
        void SetParent(T parent);
        void SetParentId(TKey? parentId);
        
    }


    public interface ITreeEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        string Name { get; }
        TKey? ParentId { get; }
        string FullPath { get; }
        string FullPathName { get; }
        int Depth { get; }
        double Sorting { get; set; }
        string Description { get; set; }
        string GetSplitString();
    }
}
