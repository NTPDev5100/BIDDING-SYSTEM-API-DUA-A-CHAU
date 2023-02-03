using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class Dbinittbl_Biddingticketandsession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"));

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "tbl_Users");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "tbl_Users");

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "tbl_Providers");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "tbl_Providers");

            migrationBuilder.CreateTable(
                name: "tbl_BiddingSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<double>(type: "float", nullable: true),
                    EndDate = table.Column<double>(type: "float", nullable: true),
                    MinimumQuantity = table.Column<int>(type: "int", nullable: true),
                    MaximumQuantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<double>(type: "float", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_BiddingSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_BiddingTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BiddingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BiddingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<double>(type: "float", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_BiddingTickets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_BiddingSessions");

            migrationBuilder.DropTable(
                name: "tbl_BiddingTickets");

            migrationBuilder.AddColumn<double>(
                name: "ExpiredDate",
                table: "tbl_Users",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "tbl_Users",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.InsertData(
                table: "tbl_Users",
                columns: new[] { "Id", "Active", "Address", "Birthday", "Code", "Created", "CreatedBy", "Deleted", "Email", "ExpiredDate", "FullName", "Gender", "IdentityCard", "IdentityCardAddress", "IdentityCardDate", "IsAdmin", "Note", "Password", "Phone", "Roles", "Status", "Thumbnail", "TimeZone", "Token", "Updated", "UpdatedBy", "Username" },
                values: new object[] { new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"), true, "Thành phố Hồ Chí Minh", 0.0, null, 1663060661.0, new Guid("00000000-0000-0000-0000-000000000000"), false, "admin@acp.com", null, "ACP", 0, null, null, null, true, null, "0328ECD37433F4E11F9DF84B4A487669B3056B22", "123 456 7890", null, 1, null, null, null, null, null, "admin" });
        }
    }
}
