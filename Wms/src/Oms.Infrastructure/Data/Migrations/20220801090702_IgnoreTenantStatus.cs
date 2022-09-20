using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huayu.Oms.Infrastructure.Data.Migrations
{
    public partial class IgnoreTenantStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tenants_tenantstatus_TenantStatusId1",
                schema: "oms",
                table: "tenants");

            migrationBuilder.DropIndex(
                name: "IX_tenants_TenantStatusId1",
                schema: "oms",
                table: "tenants");

            migrationBuilder.DropColumn(
                name: "TenantStatusId1",
                schema: "oms",
                table: "tenants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantStatusId1",
                schema: "oms",
                table: "tenants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tenants_TenantStatusId1",
                schema: "oms",
                table: "tenants",
                column: "TenantStatusId1");

            migrationBuilder.AddForeignKey(
                name: "FK_tenants_tenantstatus_TenantStatusId1",
                schema: "oms",
                table: "tenants",
                column: "TenantStatusId1",
                principalSchema: "oms",
                principalTable: "tenantstatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
