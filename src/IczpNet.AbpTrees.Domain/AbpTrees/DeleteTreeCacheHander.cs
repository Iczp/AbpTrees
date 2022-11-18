//using System.Threading.Tasks;
//using Volo.Abp.DependencyInjection;
//using Volo.Abp.Domain.Services;
//using Volo.Abp.EventBus;

//namespace IczpNet.AbpTrees
//{
//    public abstract class DeleteTreeCacheHander<TEntity, TTreeOutput> : DomainService, ILocalEventHandler<DeleteTreeCacheEto>, ITransientDependency
//        where TEntity : ITreeEntity<TEntity>
//        where TTreeOutput : ITreeInfo
//    {
//        protected ITreeManager<TEntity, TTreeOutput> TreeManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TTreeOutput>>();
//        public async Task HandleEventAsync(DeleteTreeCacheEto eventData)
//        {
//            await TreeManager.RemoveCacheAsync();
//        }
//    }
//}
