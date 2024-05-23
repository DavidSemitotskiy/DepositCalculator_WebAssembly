using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DepositCalculator.Migrations.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepositCalculations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Percent = table.Column<decimal>(type: "decimal(8,5)", precision: 8, scale: 5, nullable: false),
                    PeriodInMonths = table.Column<int>(type: "int", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyDepositCalculations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonthNumber = table.Column<int>(type: "int", nullable: false),
                    DepositByMonth = table.Column<decimal>(type: "decimal(17,9)", precision: 17, scale: 9, nullable: false),
                    TotalDepositAmount = table.Column<decimal>(type: "decimal(17,9)", precision: 17, scale: 9, nullable: false),
                    DepositCalculationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyDepositCalculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyDepositCalculations_DepositCalculations_DepositCalculationId",
                        column: x => x.DepositCalculationId,
                        principalTable: "DepositCalculations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyDepositCalculations_DepositCalculationId",
                table: "MonthlyDepositCalculations",
                column: "DepositCalculationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyDepositCalculations");

            migrationBuilder.DropTable(
                name: "DepositCalculations");
        }
    }
}
