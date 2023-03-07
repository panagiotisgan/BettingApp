using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Betting.DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    MatchDate = table.Column<DateTime>(type: "date", nullable: false),
                    MatchTime = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    TeamA = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    TeamB = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Sport = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchesOdds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    Specifier = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Odd = table.Column<decimal>(type: "decimal(7,3)", precision: 7, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchesOdds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchesOdds_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchesOdds_MatchId",
                table: "MatchesOdds",
                column: "MatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchesOdds");

            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}
