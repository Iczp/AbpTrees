using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace IczpNet.AbpTrees.Dtos
{
    public class TreeGetListInput<TKey> : PagedAndSortedResultRequestDto, ITreeGetListInput<TKey> where TKey : struct
    {
        [DefaultValue(false)]
        public virtual bool IsEnabledParentId { get; set; }

        [DefaultValue(null)]
        public virtual int? Depth { get; set; }

        [DefaultValue(null)]
        public virtual TKey? ParentId { get; set; }

        [DefaultValue(null)]
        public virtual string Keyword { get; set; }
    }
}
