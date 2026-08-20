using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tongkangku_be.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectionReasonToRentalRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "rental_requests",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "rental_requests");
        }
    }
}
