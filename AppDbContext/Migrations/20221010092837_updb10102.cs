using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class updb10102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechnicalValue",
                table: "tbl_Products");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1665394117.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TechnicalValue",
                table: "tbl_Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1665391797.0);
        }
    }
}
