using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class updatetbluserandtblprovider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "token",
                table: "tbl_Users",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "expiredDate",
                table: "tbl_Users",
                newName: "ExpiredDate");

            migrationBuilder.AddColumn<double>(
                name: "ExpiredDate",
                table: "tbl_Providers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "tbl_Providers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663060661.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "tbl_Providers");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "tbl_Providers");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "tbl_Users",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "ExpiredDate",
                table: "tbl_Users",
                newName: "expiredDate");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663060084.0);
        }
    }
}
