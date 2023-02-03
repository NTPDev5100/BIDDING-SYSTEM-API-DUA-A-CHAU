using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class DbUpdatetbl_bidding3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "tbl_Biddings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663238668.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "tbl_Biddings");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663232015.0);
        }
    }
}
