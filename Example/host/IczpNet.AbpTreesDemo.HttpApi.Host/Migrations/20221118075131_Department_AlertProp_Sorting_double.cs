using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IczpNet.AbpTreesDemo.Migrations
{
    public partial class Department_AlertProp_Sorting_double : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Sorting",
                table: "AbpTreesDemoDepartment",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Sorting",
                table: "AbpTreesDemoDepartment",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
