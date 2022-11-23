using System.Collections.Generic;

namespace IczpNet.AbpTrees
{
    public class TreeWithChildsInfo<T, TKey> : TreeInfo<TKey>, ITreeWithChildsInfo<T>
        //where T : class, ITreeInfo<TKey>
        where TKey : struct
    {
        public virtual List<T> Childs { get; set; }

    }
}
