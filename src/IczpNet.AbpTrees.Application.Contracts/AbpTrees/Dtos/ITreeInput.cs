using System;

namespace IczpNet.AbpTrees.Dtos
{
    public interface ITreeInput<TKey> where TKey : struct
    {
        string Name { get; set; }
        TKey? ParentId { get; set; }
    }
}