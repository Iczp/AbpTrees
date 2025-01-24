using System.Collections.Generic;

namespace IczpNet.AbpTrees.Dtos;

public interface ITreeGetListInput<TKey> where TKey : struct
{
    bool IsEnabledParentId { get; set; }

    List<int> DepthList { get; set; }

    TKey? ParentId { get; set; }

    string Keyword { get; set; }
}
