using IczpNet.AbpTrees.Dtos;
using IczpNet.AbpTrees.Statics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTrees
{
    public abstract class TreeAppService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput, TTreeInfo> :
        TreeAppService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput>,
        ITreeAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput, TTreeInfo>
        where TEntity : class, ITreeEntity<TEntity, TKey>
        where TKey : struct
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
        where TGetListInput : ITreeGetListInput<TKey>
        where TCreateInput : ITreeInput<TKey>
        where TUpdateInput : ITreeInput<TKey>
        where TTreeInfo : ITreeInfo<TKey>

    {
        protected new ITreeManager<TEntity, TKey, TTreeInfo> TreeManager { get; }
        //protected TreeAppService(IRepository<TEntity, TKey> repository) : base(repository) { }

        public TreeAppService(
            IRepository<TEntity, TKey> repository,
            ITreeManager<TEntity, TKey, TTreeInfo> treeManager) : base(repository, treeManager)
        {
            TreeManager = treeManager;
        }

        //[HttpGet]
        public virtual Task<TTreeInfo> GetItemByCacheAsync(TKey id)
        {
            return TreeManager.GetItemByCacheAsync(id);
        }

        //[HttpGet]
        public virtual Task<List<TTreeInfo>> GetManayByCacheAsync(List<TKey> idList)
        {
            return TreeManager.GetManyByCacheAsync(idList);
        }

        //[HttpGet]
        [RemoteService]
        public virtual async Task<PagedResultDto<TTreeInfo>> GetAllByCacheAsync(TreeGetListInput<TKey> input)
        {
            await CheckGetListPolicyAsync();

            var query = await CreateFilteredQueryByCacheAsync(input);

            query = ApplyDefaultSortingByCache(query);

            var totalCount = await AsyncExecuter.CountAsync(query);

            var items = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<TTreeInfo>(totalCount, items);
        }

        protected virtual async Task<IQueryable<TTreeInfo>> CreateFilteredQueryByCacheAsync(TreeGetListInput<TKey> input)
        {
            await Task.Yield();

            var query = (await TreeManager.GetAllByCacheAsync())
                .AsQueryable()
                .WhereIf(input.DepthList != null && input.DepthList.Any(), x => input.DepthList.Contains(x.Depth))
                .WhereIf(input.IsEnabledParentId, x => x.ParentId.Equals(input.ParentId))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword))
                ;
            return query;
        }

        protected virtual IQueryable<TTreeInfo> ApplyDefaultSortingByCache(IQueryable<TTreeInfo> query)
        {
            return query;
        }
    }


    public abstract class TreeAppService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput> :
        CrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>,
        ITreeAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
        where TEntity : class, ITreeEntity<TEntity, TKey>
        where TKey : struct
        where TGetOutputDto : IEntityDto<TKey>
        where TGetListOutputDto : IEntityDto<TKey>
        where TGetListInput : ITreeGetListInput<TKey>
        where TCreateInput : ITreeInput<TKey>
        where TUpdateInput : ITreeInput<TKey>

    {
        protected virtual string RepairDataPolicyName { get; set; }

        protected virtual ITreeManager<TEntity, TKey> TreeManager { get; }

        //public TreeAppService(IRepository<TEntity, TKey> repository) : base(repository)
        //{
        //    //TreeManager = LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TKey>>();
        //}

        public TreeAppService(
            IRepository<TEntity, TKey> repository,
            ITreeManager<TEntity, TKey> treeManager) : base(repository)
        {
            TreeManager = treeManager;
        }



        //[HttpGet]
        public override async Task<TGetOutputDto> GetAsync(TKey id)
        {
            await CheckGetPolicyAsync(id);

            var entity = await GetEntityByIdAsync(id);

            return await MapToGetOutputDtoAsync(entity);
        }

        protected virtual Task CheckGetPolicyAsync(TKey id)
        {
            return CheckGetPolicyAsync();
        }

        //[HttpGet]
        public virtual async Task<List<TGetOutputDto>> GetManyAsync(List<TKey> idList)
        {
            var list = new List<TGetOutputDto>();

            foreach (var id in idList)
            {
                list.Add(await GetAsync(id));
            }
            return list;
        }

        protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
        {
            return query.OrderByDescending(x => x.Sorting);
        }

        //[HttpGet]
        public override async Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {
            await CheckGetListPolicyAsync(input);

            var query = await CreateFilteredQueryAsync(input);

            var totalCount = await AsyncExecuter.CountAsync(query);

            var entityDtos = new List<TGetListOutputDto>();

            if (totalCount > 0)
            {
                query = ApplySorting(query, input);

                query = ApplyPaging(query, input);

                var entities = await AsyncExecuter.ToListAsync(query);

                entityDtos = await MapToGetListOutputDtosAsync(entities);
            }

            return new PagedResultDto<TGetListOutputDto>(totalCount, entityDtos);
        }

        protected virtual Task CheckGetListPolicyAsync(TGetListInput input)
        {
            return CheckGetListPolicyAsync();
        }

        protected override async Task<IQueryable<TEntity>> CreateFilteredQueryAsync(TGetListInput input)
        {
            Assert.If(!input.IsEnabledParentId && input.ParentId != null, "When [IsEnabledParentId]=false,then [ParentId] != null");

            return (await base.CreateFilteredQueryAsync(input))
                .WhereIf(input.DepthList != null && input.DepthList.Any(), x => input.DepthList.Contains(x.Depth))
                .WhereIf(input.IsEnabledParentId, x => x.ParentId.Equals(input.ParentId))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword))
               ;
        }


        //[HttpPost]
        public override async Task<TGetOutputDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePolicyAsync(input);

            var inputEntity = MapToEntity(input);

            inputEntity.SetName(input.Name);

            //inputEntity.SetParentId(input.ParentId);

            var entity = await TreeManager.CreateAsync(inputEntity, true);

            return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
        }

        protected virtual Task CheckCreatePolicyAsync(TCreateInput input)
        {
            return CheckCreatePolicyAsync();
        }

        //[HttpPost]
        public override async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            await CheckUpdatePolicyAsync(id, input);

            var entity = await GetEntityByIdAsync(id);

            await MapToEntityAsync(input, entity);

            entity.SetName(input.Name);

            //entity.SetParentId(input.ParentId);

            await TreeManager.UpdateAsync(entity, input.ParentId);

            return await MapToGetOutputDtoAsync(entity);
        }

        protected virtual Task CheckUpdatePolicyAsync(TKey id, TUpdateInput input)
        {
            return CheckUpdatePolicyAsync();
        }

        //[HttpPost]
        public override async Task DeleteAsync(TKey id)
        {
            await CheckDeletePolicyAsync(id);

            await TreeManager.DeleteAsync(id);
        }

        protected virtual Task CheckDeletePolicyAsync(TKey id)
        {
            return CheckDeletePolicyAsync(id);
        }

        //[HttpPost]
        public virtual async Task DeleteManyAsync(List<TKey> idList)
        {
            foreach (var id in idList)
            {
                await DeleteAsync(id);
            }
        }

        //[HttpPost]
        public virtual async Task<string> RepairDataAsync(int maxResultCount = 100, int skinCount = 0)
        {
            await CheckRepairDataPolicyAsync(maxResultCount, skinCount);

            var stopwatch = Stopwatch.StartNew();

            var affectsCount = await TreeManager.RepairDataAsync(maxResultCount, skinCount);

            return $"AffectsCount:{affectsCount},MaxResultCount:{maxResultCount},SkinCount:{skinCount},stopwatch:{stopwatch.ElapsedMilliseconds}ms";
        }

        protected virtual async Task CheckRepairDataPolicyAsync(int maxResultCount = 100, int skinCount = 0)
        {
            await CheckPolicyAsync(RepairDataPolicyName);
        }
    }
}
