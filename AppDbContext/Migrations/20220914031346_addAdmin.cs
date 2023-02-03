using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class addAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tbl_Users",
                columns: new[] { "Id", "Active", "Address", "Birthday", "Code", "Created", "CreatedBy", "Deleted", "Email", "FullName", "Gender", "IdentityCard", "IdentityCardAddress", "IdentityCardDate", "IsAdmin", "Note", "Password", "Phone", "Roles", "Status", "Thumbnail", "TimeZone", "Updated", "UpdatedBy", "Username" },
                values: new object[] { new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"), true, "Thành phố Hồ Chí Minh", 0.0, null, 1663125226.0, new Guid("00000000-0000-0000-0000-000000000000"), false, "admin@acp.com", "ACP", 0, null, null, null, true, null, "0328ECD37433F4E11F9DF84B4A487669B3056B22", "123 456 7890", null, 1, null, null, null, null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"));
        }
    }
}
