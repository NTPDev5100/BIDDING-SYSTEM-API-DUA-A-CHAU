using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class upbidding1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bidding",
                table: "tbl_BiddingTickets");

            migrationBuilder.DropColumn(
                name: "BiddingName",
                table: "tbl_BiddingTickets");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "tbl_BiddingTickets");

            migrationBuilder.AddColumn<Guid>(
                name: "BiddingId",
                table: "tbl_BiddingTickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663320961.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiddingId",
                table: "tbl_BiddingTickets");

            migrationBuilder.AddColumn<string>(
                name: "Bidding",
                table: "tbl_BiddingTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BiddingName",
                table: "tbl_BiddingTickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "tbl_BiddingTickets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663303057.0);
        }
    }
}
