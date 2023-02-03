using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class dbupBiddingSession3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Product",
                table: "tbl_BiddingSessions",
                newName: "ProductName");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663741699.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "tbl_BiddingSessions",
                newName: "Product");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663741561.0);
        }
    }
}
