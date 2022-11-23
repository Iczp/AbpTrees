using System.Collections.Generic;

namespace IczpNet.AbpTrees
{
    public interface ITreeWithChildsInfo<T> //: ITreeInfo<TKey> where TKey : struct
    {
        List<T> Childs { get; set; }
    }
}
