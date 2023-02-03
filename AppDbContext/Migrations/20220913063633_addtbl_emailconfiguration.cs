using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class addtbl_emailconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_EmailConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SmtpServer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    EnableSsl = table.Column<bool>(type: "bit", nullable: false),
                    ConnectType = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    userName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ItemSendCount = table.Column<int>(type: "int", nullable: false),
                    TimeSend = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<double>(type: "float", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_EmailConfigurations", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663050993.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_EmailConfigurations");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1662974995.0);
        }
    }
}
