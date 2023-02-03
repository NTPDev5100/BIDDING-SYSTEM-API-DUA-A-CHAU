using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class upUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                columns: new[] { "Created", "Password" },
                values: new object[] { 1663302826.0, "CB16368C3CE5570F725BB3FD943550F6F545A008" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                columns: new[] { "Created", "Password" },
                values: new object[] { 1663302700.0, "0328ECD37433F4E11F9DF84B4A487669B3056B22" });
        }
    }
}
