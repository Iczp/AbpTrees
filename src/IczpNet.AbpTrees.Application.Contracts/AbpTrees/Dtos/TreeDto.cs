using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTrees.AbpTrees.Dtos;

public class TreeDto<TKey> : EntityDto<TKey>, ITreeDto<TKey> where TKey : struct
{
    public virtual TKey? ParentId { get; set; }

    public virtual string Name { get; set; }

    public override string ToString()
    {
        return $"[TreeDto: {GetType().Name}] Id = {Id}, ParentId = {ParentId}, Name = {Name}";
    }
}
