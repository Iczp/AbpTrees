using System;

namespace IczpNet.AbpTrees.Dtos
{
    public interface ITreeInput
    {
        string Name { get; set; }
        Guid? ParentId { get; set; }
    }
}