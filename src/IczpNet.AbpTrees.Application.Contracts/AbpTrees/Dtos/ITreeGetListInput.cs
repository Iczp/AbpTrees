namespace IczpNet.AbpTrees.Dtos
{
    public interface ITreeGetListInput<TKey> where TKey : struct
    {
        bool IsEnabledParentId { get; set; }

        int? Depth { get; set; }

        TKey? ParentId { get; set; }

        string Keyword { get; set; }
    }
}
