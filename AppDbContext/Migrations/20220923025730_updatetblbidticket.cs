using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class updatetblbidticket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiddingSessionName",
                table: "tbl_BiddingTickets");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663901850.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BiddingSessionName",
                table: "tbl_BiddingTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663899481.0);
        }
    }
}
