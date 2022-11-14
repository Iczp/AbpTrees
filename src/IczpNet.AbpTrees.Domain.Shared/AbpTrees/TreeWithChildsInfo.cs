using System.Collections.Generic;

namespace IczpNet.AbpTrees
{
    /// <summary>
    /// TreeWithChildsInfo
    /// </summary>
    public class TreeWithChildsInfo<T> : TreeInfo, ITreeWithChildsInfo<T>
    {
        public virtual List<T> Childs { get; set; }

    }
}
