using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tongkangku_be.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_cargos_rental_contract_ContractId",
                table: "contract_cargos");

            migrationBuilder.DropForeignKey(
                name: "FK_laytime_records_rental_contract_ContractId",
                table: "laytime_records");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_contract_rental_requests_RentalRequestId",
                table: "rental_contract");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_contract_users_OwnerId",
                table: "rental_contract");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rental_contract",
                table: "rental_contract");

            migrationBuilder.RenameTable(
                name: "rental_contract",
                newName: "rental_contracts");

            migrationBuilder.RenameIndex(
                name: "IX_rental_contract_RentalRequestId",
                table: "rental_contracts",
                newName: "IX_rental_contracts_RentalRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_rental_contract_OwnerId",
                table: "rental_contracts",
                newName: "IX_rental_contracts_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rental_contracts",
                table: "rental_contracts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_cargos_rental_contracts_ContractId",
                table: "contract_cargos",
                column: "ContractId",
                principalTable: "rental_contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_laytime_records_rental_contracts_ContractId",
                table: "laytime_records",
                column: "ContractId",
                principalTable: "rental_contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_contracts_rental_requests_RentalRequestId",
                table: "rental_contracts",
                column: "RentalRequestId",
                principalTable: "rental_requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_contracts_users_OwnerId",
                table: "rental_contracts",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_cargos_rental_contracts_ContractId",
                table: "contract_cargos");

            migrationBuilder.DropForeignKey(
                name: "FK_laytime_records_rental_contracts_ContractId",
                table: "laytime_records");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_contracts_rental_requests_RentalRequestId",
                table: "rental_contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_contracts_users_OwnerId",
                table: "rental_contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rental_contracts",
                table: "rental_contracts");

            migrationBuilder.RenameTable(
                name: "rental_contracts",
                newName: "rental_contract");

            migrationBuilder.RenameIndex(
                name: "IX_rental_contracts_RentalRequestId",
                table: "rental_contract",
                newName: "IX_rental_contract_RentalRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_rental_contracts_OwnerId",
                table: "rental_contract",
                newName: "IX_rental_contract_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rental_contract",
                table: "rental_contract",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_cargos_rental_contract_ContractId",
                table: "contract_cargos",
                column: "ContractId",
                principalTable: "rental_contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_laytime_records_rental_contract_ContractId",
                table: "laytime_records",
                column: "ContractId",
                principalTable: "rental_contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_contract_rental_requests_RentalRequestId",
                table: "rental_contract",
                column: "RentalRequestId",
                principalTable: "rental_requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_contract_users_OwnerId",
                table: "rental_contract",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
