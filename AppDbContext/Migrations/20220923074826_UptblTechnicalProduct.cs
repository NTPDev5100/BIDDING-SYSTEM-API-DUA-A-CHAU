using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class UptblTechnicalProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechnicalOptionId",
                table: "tbl_TechnicalProducts");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "tbl_TechnicalProducts",
                newName: "TechnicalValue");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663919306.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TechnicalValue",
                table: "tbl_TechnicalProducts",
                newName: "Content");

            migrationBuilder.AddColumn<Guid>(
                name: "TechnicalOptionId",
                table: "tbl_TechnicalProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663917430.0);
        }
    }
}
