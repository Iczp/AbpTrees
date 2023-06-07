using IczpNet.AbpTrees.Dtos;

namespace IczpNet.AbpTrees.AbpTrees.Dtos
{
    public class TreeInput<TKey> : ITreeInput<TKey> where TKey : struct
    {
        public virtual TKey? ParentId { get; set; }

        public virtual string Name { get; set; }

        public override string ToString()
        {
            return $"[TreeInput: {GetType().Name}] ParentId = {ParentId}";
        }
    }
}
