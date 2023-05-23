using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IczpNet.AbpTreesDemo.Migrations
{
    /// <inheritdoc />
    public partial class TreeEntity_AddProp_ChildrenCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChildrenCount",
                table: "AbpTreesDemoDepartment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildrenCount",
                table: "AbpTreesDemoDepartment");
        }
    }
}
