using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tongkangku_be.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "rental_offers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "rental_contracts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "rental_contracts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalSettlementAmount",
                table: "rental_contracts",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalLaytimeAdjustment",
                table: "rental_contracts",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "rental_offers");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "rental_contracts");

            migrationBuilder.DropColumn(
                name: "FinalSettlementAmount",
                table: "rental_contracts");

            migrationBuilder.DropColumn(
                name: "TotalLaytimeAdjustment",
                table: "rental_contracts");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "rental_contracts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}
