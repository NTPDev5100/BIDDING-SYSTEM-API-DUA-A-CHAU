using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class updatetblTechnicalOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFile",
                table: "tbl_TechnicalOptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663916154.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFile",
                table: "tbl_TechnicalOptions");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663915336.0);
        }
    }
}
