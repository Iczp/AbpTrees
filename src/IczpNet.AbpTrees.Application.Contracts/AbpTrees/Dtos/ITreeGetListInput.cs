using System;

namespace IczpNet.AbpTrees.Dtos
{
    public interface ITreeGetListInput
    {
        bool IsEnabledParentId { get; set; }

        int? Depth { get; set; }

        Guid? ParentId { get; set; }

        string Keyword { get; set; }
    }
}
