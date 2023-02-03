using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class dbinit1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Menu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<double>(type: "float", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<double>(type: "float", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Birthday = table.Column<double>(type: "float", nullable: true),
                    IdentityCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdentityCardDate = table.Column<double>(type: "float", nullable: true),
                    IdentityCardAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: true),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeZone = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<double>(type: "float", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "tbl_Users",
                columns: new[] { "Id", "Active", "Address", "Birthday", "Code", "Created", "CreatedBy", "Deleted", "Email", "FirstName", "FullName", "Gender", "IdentityCard", "IdentityCardAddress", "IdentityCardDate", "IsAdmin", "LastName", "Note", "Password", "Phone", "Roles", "Status", "Thumbnail", "TimeZone", "Updated", "UpdatedBy", "Username" },
                values: new object[] { new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"), true, "Thành phố Hồ Chí Minh", 0.0, null, 1662526343.0, new Guid("00000000-0000-0000-0000-000000000000"), false, "admin@acp.com", null, "ACP", 0, null, null, null, true, null, null, "0328ECD37433F4E11F9DF84B4A487669B3056B22", "123 456 7890", null, 1, null, null, null, null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Menu");

            migrationBuilder.DropTable(
                name: "tbl_Role");

            migrationBuilder.DropTable(
                name: "tbl_Users");
        }
    }
}
