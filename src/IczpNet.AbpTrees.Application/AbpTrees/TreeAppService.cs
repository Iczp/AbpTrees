using IczpNet.AbpTrees.Dtos;
using IczpNet.AbpTrees.Statics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace IczpNet.AbpTrees
{
    public abstract class TreeAppService<
        TEntity,
        TGetOutputDto,
        TGetListOutputDto,
        TGetListInput,
        TCreateInput,
        TUpdateInput,
        TTreeInfo,
        TTreeWithChildsDto,
        TTreeWithParentDto>
        :
        TreeAppService<
            TEntity,
            TGetOutputDto,
            TGetListOutputDto,
            TGetListInput,
            TCreateInput,
            TUpdateInput,
            TTreeInfo,
            TTreeWithChildsDto>
        ,
        ITreeAppService<
        TTreeInfo,
        TTreeWithChildsDto,
        TTreeWithParentDto>
        where TEntity : class, ITreeEntity<TEntity>, ITreeEntity
        where TGetOutputDto : IEntityDto<Guid>
        where TGetListOutputDto : IEntityDto<Guid>
        where TGetListInput : ITreeGetListInput
        where TCreateInput : ITreeInput
        where TUpdateInput : ITreeInput
        where TTreeInfo : ITreeInfo
        where TTreeWithChildsDto : ITreeWithChildsInfo<TTreeWithChildsDto>
        where TTreeWithParentDto : ITreeWithParentInfo<TTreeWithParentDto>
    {
        //protected override ITreeManager<TEntity, TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto> TreeManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto>>();
        protected TreeAppService(IRepository<TEntity, Guid> repository) : base(repository) { }

        [HttpGet]
        public virtual async Task<TTreeWithParentDto> GetWithParentAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var entity = await base.GetEntityByIdAsync(id);

            return ObjectMapper.Map<TEntity, TTreeWithParentDto>(entity);
        }
    }
    public abstract class TreeAppService<
        TEntity,
        TGetOutputDto,
        TGetListOutputDto,
        TGetListInput,
        TCreateInput,
        TUpdateInput,
        TTreeInfo,
        TTreeWithChildsDto>
        :
        TreeAppService<
            TEntity,
            TGetOutputDto,
            TGetListOutputDto,
            TGetListInput,
            TCreateInput,
            TUpdateInput,
            TTreeInfo>
        ,
        ITreeAppService<
        TTreeInfo,
        TTreeWithChildsDto>
        where TEntity : class, ITreeEntity<TEntity>, ITreeEntity
        where TGetOutputDto : IEntityDto<Guid>
        where TGetListOutputDto : IEntityDto<Guid>
        where TGetListInput : ITreeGetListInput
        where TCreateInput : ITreeInput
        where TUpdateInput : ITreeInput
        where TTreeInfo : ITreeInfo
        where TTreeWithChildsDto : ITreeWithChildsInfo<TTreeWithChildsDto>
    {
        protected ITreeManager<TEntity, TTreeInfo, TTreeWithChildsDto> TreeWithChildsManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TTreeInfo, TTreeWithChildsDto>>();
        protected TreeAppService(IRepository<TEntity, Guid> repository) : base(repository) { }

        [HttpGet]
        public virtual async Task<List<TTreeWithChildsDto>> GetAllListWithChildsAsync(Guid? ParentId, bool IsImportAllChilds)
        {
            await CheckGetListPolicyAsync();

            return await TreeWithChildsManager.GetAllListWithChildsAsync(ParentId, IsImportAllChilds);
        }
        [HttpGet]
        public virtual async Task<List<TTreeWithChildsDto>> GetRootListAsync(List<Guid> idList)
        {
            await CheckGetPolicyAsync();

            return await TreeWithChildsManager.GetRootListAsync(idList);
        }
    }
    public abstract class TreeAppService<
        TEntity,
        TGetOutputDto,
        TGetListOutputDto,
        TGetListInput,
        TCreateInput,
        TUpdateInput,
        TTreeInfo>
        :
        TreeAppService<
            TEntity,
            TGetOutputDto,
            TGetListOutputDto,
            TGetListInput,
            TCreateInput,
            TUpdateInput>
        ,
        ITreeAppService<TTreeInfo>
        where TEntity : class, ITreeEntity<TEntity>, ITreeEntity
        where TGetOutputDto : IEntityDto<Guid>
        where TGetListOutputDto : IEntityDto<Guid>
        where TGetListInput : ITreeGetListInput
        where TCreateInput : ITreeInput
        where TUpdateInput : ITreeInput
        where TTreeInfo : ITreeInfo
    {
        protected ITreeManager<TEntity, TTreeInfo> TreeCacheManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity, TTreeInfo>>();
        protected TreeAppService(IRepository<TEntity, Guid> repository) : base(repository) { }


        [HttpGet]
        public virtual async Task<List<TTreeInfo>> GetAllListByCacheAsync()
        {
            await CheckGetListPolicyAsync();

            return await TreeCacheManager.GetAllListByCacheAsync();
        }
    }


    public abstract class TreeAppService<
        TEntity,
        TGetOutputDto,
        TGetListOutputDto,
        TGetListInput,
        TCreateInput,
        TUpdateInput>
        :
        CrudAppService<
            TEntity,
            TGetOutputDto,
            TGetListOutputDto,
            Guid,
            TGetListInput,
            TCreateInput,
            TUpdateInput>
        //:ITreeAppService<TTreeInfo, TTreeWithChildsDto, TTreeWithParentDto>
    where TEntity : class, ITreeEntity<TEntity>, ITreeEntity
    where TGetOutputDto : IEntityDto<Guid>
    where TGetListOutputDto : IEntityDto<Guid>
    where TGetListInput : ITreeGetListInput
    where TCreateInput : ITreeInput
    where TUpdateInput : ITreeInput
    {
        protected virtual string RepairDataPolicyName { get; set; }

        protected virtual ITreeManager<TEntity> TreeManager => LazyServiceProvider.LazyGetRequiredService<ITreeManager<TEntity>>();

        public TreeAppService(IRepository<TEntity, Guid> repository) : base(repository) { }

        protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
        {
            return query.OrderByDescending(x => x.Sorting);
        }

        [HttpGet]
        public override Task<TGetOutputDto> GetAsync(Guid id)
        {
            return base.GetAsync(id);
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
                .WhereIf(input.Depth.HasValue, x => x.Depth == input.Depth)
                .WhereIf(input.IsEnabledParentId, x => x.ParentId == input.ParentId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword))
               ;
        }


        [HttpPost]
        public override async Task<TGetOutputDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePolicyAsync();

            var inputEntity = MapToEntity(input);

            inputEntity.FillCreate(GuidGenerator.Create(), input.Name, input.ParentId);

            var entity = await TreeManager.CreateAsync(inputEntity);

            return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
        }

        [HttpPost]
        public override async Task<TGetOutputDto> UpdateAsync(Guid id, TUpdateInput input)
        {
            await CheckUpdatePolicyAsync();

            var entity = await GetEntityByIdAsync(id);

            await MapToEntityAsync(input, entity);

            entity.FillUpdate(input.Name, input.ParentId);

            await TreeManager.UpdateAsync(entity);

            return await MapToGetOutputDtoAsync(entity);
        }

        [HttpPost]
        public override async Task DeleteAsync(Guid id)
        {
            await CheckDeletePolicyAsync();

            await TreeManager.DeleteAsync(id);
        }

        [HttpPost]
        public virtual async Task RepairDataAsync()
        {
            await CheckRepairDataPolicyAsync();

            await TreeManager.RepairDataAsync();
        }

        protected virtual async Task CheckRepairDataPolicyAsync()
        {
            await CheckPolicyAsync(RepairDataPolicyName);
        }

    }
}
