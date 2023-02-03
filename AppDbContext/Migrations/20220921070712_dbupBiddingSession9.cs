using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDbContext.Migrations
{
    public partial class dbupBiddingSession9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_BiddingSessions");

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663744032.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_BiddingSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    BiddingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<double>(type: "float", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EndDate = table.Column<double>(type: "float", nullable: true),
                    MaximumQuantity = table.Column<int>(type: "int", nullable: true),
                    MinimumQuantity = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Updated = table.Column<double>(type: "float", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_BiddingSessions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "tbl_Users",
                keyColumn: "Id",
                keyValue: new Guid("0debff1d-ac80-4e2d-be24-3151b26f2176"),
                column: "Created",
                value: 1663743953.0);
        }
    }
}
