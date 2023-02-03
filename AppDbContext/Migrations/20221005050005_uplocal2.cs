using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class uplocal2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tbl_EmailConfigurations",
                columns: new[] { "Id", "Active", "ConnectType", "Created", "CreatedBy", "Deleted", "DisplayName", "Email", "EnableSsl", "ItemSendCount", "Password", "Port", "SmtpServer", "TimeSend", "Updated", "UpdatedBy", "userName" },
                values: new object[] { new Guid("72712f29-8693-42d1-97aa-08da9153b528"), true, 1, 1664946005.0, new Guid("00000000-0000-0000-0000-000000000000"), false, "ACP", "acponesignal@gmail.com", true, 1, "ouaypciwmxbqbcxn", 587, "smtp.gmail.com", 1000, null, null, "acponesignal@gmail.com" });

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1664946005.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_EmailConfigurations",
                keyColumn: "Id",
                keyValue: new Guid("72712f29-8693-42d1-97aa-08da9153b528"));

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1664939284.0);
        }
    }
}
