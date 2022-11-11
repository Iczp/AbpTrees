using System.Collections.Generic;

namespace IczpNet.AbpTrees
{
    public interface ITreeWithChildsInfo<T> : ITreeInfo
    {
        List<T> Childs { get; set; }
    }
}
