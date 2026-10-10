using System;
using System.Collections.Generic;
using IczpNet.AbpTreesDemo.Departments;
using IczpNet.AbpTreesDemo.Departments.Dtos;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace IczpNet.AbpTreesDemo.Departments;

public class DepartmentMapperly_Tests
{
    [Fact]
    public void Entity_And_Tree_Dtos_Keep_Their_Fields()
    {
        var child = new TestDepartment();
        child.SetName("Child");
        child.SetChilds([]);

        var source = new TestDepartment
        {
            ParentId = Guid.NewGuid(),
            Sorting = 12.5,
            Description = "Example"
        };
        source.SetName("Root");
        source.SetChildrenCount(9);
        source.SetChilds([child]);

        var dto = new DepartmentToDepartmentDtoMapper().Map(source);
        dto.Name.ShouldBe("Root");
        dto.Sorting.ShouldBe(12.5);
        dto.Description.ShouldBe("Example");
        dto.ParentId.ShouldBe(source.ParentId);

        var withChilds = new DepartmentToDepartmentWithChildsDtoMapper().Map(source);
        withChilds.ChildsCount.ShouldBe(1);
        withChilds.ChildrenCount.ShouldBe(9);
        withChilds.Childs.Count.ShouldBe(1);
        withChilds.Childs[0].Name.ShouldBe("Child");
        withChilds.Childs[0].ChildsCount.ShouldBe(0);

        var treeInfo = new DepartmentToDepartmentInfoMapper().Map(source);
        treeInfo.ChildrenCount.ShouldBe(9);

        var treeDto = new DepartmentInfoToDepartmentWithChildsDtoMapper().Map(treeInfo);
        treeDto.Name.ShouldBe("Root");
        treeDto.ChildrenCount.ShouldBe(9);
        treeDto.Childs.ShouldBeNull();
    }

    [Fact]
    public void Create_And_Update_Inputs_Keep_Editable_Fields()
    {
        var parentId = Guid.NewGuid();
        var create = new DepartmentCreateInput
        {
            ParentId = parentId,
            Name = "Create",
            Sorting = 3.5,
            Description = "New"
        };

        var entity = new DepartmentCreateInputToDepartmentMapper().Map(create);
        entity.ParentId.ShouldBe(parentId);
        entity.Sorting.ShouldBe(3.5);
        entity.Description.ShouldBe("New");

        new DepartmentUpdateInputToDepartmentMapper().Map(
            new DepartmentUpdateInput { Sorting = 7.5, Description = "Changed" },
            entity);
        entity.Sorting.ShouldBe(7.5);
        entity.Description.ShouldBe("Changed");
    }

    private sealed class TestDepartment : Department
    {
        public void SetChilds(IEnumerable<Department> childs) => Childs = childs;
    }
}

public class DepartmentMapperlyIntegration_Tests : AbpTreesDemoApplicationTestBase
{
    [Fact]
    public void Default_ObjectMapper_Can_Map_TreeManager_Pairs()
    {
        var source = new TestDepartment { Description = "Mapped" };
        source.SetName("Root");
        source.SetChilds([]);

        var mapper = GetRequiredService<IObjectMapper>();
        mapper.Map<Department, DepartmentInfo>(source).Name.ShouldBe("Root");
        mapper.Map<Department, DepartmentWithParentDto>(source).Description.ShouldBe("Mapped");
        mapper.Map<List<Department>, List<DepartmentWithChildsDto>>([source])[0].Name.ShouldBe("Root");
    }

    private sealed class TestDepartment : Department
    {
        public void SetChilds(IEnumerable<Department> childs) => Childs = childs;
    }
}
