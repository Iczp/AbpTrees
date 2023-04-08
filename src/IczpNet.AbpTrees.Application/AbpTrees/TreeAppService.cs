using IczpNet.AbpTrees.Dtos;
using IczpNet.AbpTrees.Statics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected ITreeManager<TEntity, TKey, TTreeInfo> TreeCacheManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TKey, TTreeInfo>>();
        protected TreeAppService(IRepository<TEntity, TKey> repository) : base(repository) { }

        [HttpGet]
        public virtual Task<TTreeInfo> GetItemByCacheAsync(TKey id)
        {
            return TreeCacheManager.GetItemByCacheAsync(id);
        }

        [HttpGet]
        public virtual Task<List<TTreeInfo>> GetManayByCacheAsync(List<TKey> idList)
        {
            return TreeCacheManager.GetManyByCacheAsync(idList);
        }

        [HttpGet]
        [RemoteService]
        public virtual async Task<List<TTreeInfo>> GetAllByCacheAsync()
        {
            await CheckGetListPolicyAsync();

            return await TreeCacheManager.GetAllByCacheAsync();
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

        protected virtual ITreeManager<TEntity, TKey> TreeManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TKey>>();

        public TreeAppService(IRepository<TEntity, TKey> repository) : base(repository) { }

        protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
        {
            return query.OrderByDescending(x => x.Sorting);
        }

        [HttpGet]
        public override Task<TGetOutputDto> GetAsync(TKey id)
        {
            return base.GetAsync(id);
        }



        [HttpGet]
        public virtual async Task<List<TGetOutputDto>> GetManyAsync(List<TKey> idList)
        {
            var list = new List<TGetOutputDto>();
            foreach (var id in idList)
            {
                list.Add(await GetAsync(id));
            }
            return list;
        }

        [HttpGet]
        public override Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {
            return base.GetListAsync(input);
        }

        protected override async Task<IQueryable<TEntity>> CreateFilteredQueryAsync(TGetListInput input)
        {
            Assert.If(!input.IsEnabledParentId && input.ParentId != null, "When [IsEnabledParentId]=false,then [ParentId] != null");

            return (await base.CreateFilteredQueryAsync(input))
                .WhereIf(input.DepthList != null && input.DepthList.Any(), x => input.DepthList.Contains(x.Depth))
                .WhereIf(input.IsEnabledParentId, x => x.ParentId.Equals(input.ParentId))
               //.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword))
               ;
        }


        [HttpPost]
        public override async Task<TGetOutputDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePolicyAsync();

            var inputEntity = MapToEntity(input);

            inputEntity.SetName(input.Name);

            inputEntity.SetParentId(input.ParentId);

            var entity = await TreeManager.CreateAsync(inputEntity, true);

            return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
        }

        [HttpPost]
        public override async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await GetEntityByIdAsync(id);

            await MapToEntityAsync(input, entity);

            entity.SetName(input.Name);

            entity.SetParentId(input.ParentId);

            await TreeManager.UpdateAsync(entity);

            return await MapToGetOutputDtoAsync(entity);
        }

        [HttpPost]
        public override async Task DeleteAsync(TKey id)
        {
            await CheckDeletePolicyAsync();

            await TreeManager.DeleteAsync(id);
        }

        [HttpPost]
        public async Task DeleteManayAsync(List<TKey> idList)
        {
            foreach (var id in idList)
            {
                await DeleteAsync(id);
            }
        }

        [HttpPost]
        public virtual async Task<DateTime> RepairDataAsync()
        {
            await CheckRepairDataPolicyAsync();

            await TreeManager.RepairDataAsync();

            return Clock.Now;
        }

        protected virtual async Task CheckRepairDataPolicyAsync()
        {
            await CheckPolicyAsync(RepairDataPolicyName);
        }
    }
}
