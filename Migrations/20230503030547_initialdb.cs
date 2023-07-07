using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CCP_HW2_Project_A9210256.Migrations
{
    /// <inheritdoc />
    public partial class initialdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SportRecords",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    time = table.Column<int>(type: "int", nullable: false),
                    sportType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    heartbeat = table.Column<int>(type: "int", nullable: true),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sportDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportRecords", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SportTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sportType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportTypes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SportRecords");

            migrationBuilder.DropTable(
                name: "SportTypes");
        }
    }
}
