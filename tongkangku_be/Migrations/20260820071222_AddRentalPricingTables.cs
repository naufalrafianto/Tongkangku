using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tongkangku_be.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalPricingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rental_operational_costs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CostType = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_operational_costs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rental_pricing_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ContingencyRate = table.Column<decimal>(type: "numeric(5,4)", nullable: false),
                    TargetMargin = table.Column<decimal>(type: "numeric(5,4)", nullable: false),
                    ShortDurationMaxDays = table.Column<int>(type: "integer", nullable: false),
                    ShortDurationMultiplier = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    MediumDurationMaxDays = table.Column<int>(type: "integer", nullable: false),
                    MediumDurationMultiplier = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    LongDurationMultiplier = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_pricing_settings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rental_operational_costs_CostType",
                table: "rental_operational_costs",
                column: "CostType",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rental_operational_costs");

            migrationBuilder.DropTable(
                name: "rental_pricing_settings");
        }
    }
}
