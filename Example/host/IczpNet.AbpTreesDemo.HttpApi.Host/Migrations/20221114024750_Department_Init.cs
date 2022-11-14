using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IczpNet.AbpTreesDemo.Migrations
{
    public partial class Department_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpTreesDemoDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FullPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FullPathName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    Sorting = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTreesDemoDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpTreesDemoDepartment_AbpTreesDemoDepartment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AbpTreesDemoDepartment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpTreesDemoDepartment_CreationTime",
                table: "AbpTreesDemoDepartment",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTreesDemoDepartment_ParentId",
                table: "AbpTreesDemoDepartment",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpTreesDemoDepartment");
        }
    }
}
