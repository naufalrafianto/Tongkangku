using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tongkangku_be.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_cargo_cargo_type_CargoTypeId",
                table: "contract_cargo");

            migrationBuilder.DropForeignKey(
                name: "FK_contract_cargo_rental_contract_ContractId",
                table: "contract_cargo");

            migrationBuilder.DropForeignKey(
                name: "FK_laytime_record_rental_contract_ContractId",
                table: "laytime_record");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_contract_rental_request_RentalRequestId",
                table: "rental_contract");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_contract_user_OwnerId",
                table: "rental_contract");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_request_user_ChartererId",
                table: "rental_request");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_request_vessels_VesselId",
                table: "rental_request");

            migrationBuilder.DropForeignKey(
                name: "FK_vessels_port_PortId",
                table: "vessels");

            migrationBuilder.DropForeignKey(
                name: "FK_vessels_user_OwnerId",
                table: "vessels");

            migrationBuilder.DropForeignKey(
                name: "FK_vessels_vessel_category_CategoryId",
                table: "vessels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vessel_category",
                table: "vessel_category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rental_request",
                table: "rental_request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_port",
                table: "port");

            migrationBuilder.DropPrimaryKey(
                name: "PK_laytime_record",
                table: "laytime_record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contract_cargo",
                table: "contract_cargo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cargo_type",
                table: "cargo_type");

            migrationBuilder.DropColumn(
                name: "LaytimeHours",
                table: "rental_contract");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "rental_contract");

            migrationBuilder.RenameTable(
                name: "vessel_category",
                newName: "vessel_categories");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "rental_request",
                newName: "rental_requests");

            migrationBuilder.RenameTable(
                name: "port",
                newName: "ports");

            migrationBuilder.RenameTable(
                name: "laytime_record",
                newName: "laytime_records");

            migrationBuilder.RenameTable(
                name: "contract_cargo",
                newName: "contract_cargos");

            migrationBuilder.RenameTable(
                name: "cargo_type",
                newName: "cargo_types");

            migrationBuilder.RenameIndex(
                name: "IX_user_Email",
                table: "users",
                newName: "IX_users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_rental_request_VesselId",
                table: "rental_requests",
                newName: "IX_rental_requests_VesselId");

            migrationBuilder.RenameIndex(
                name: "IX_rental_request_ChartererId",
                table: "rental_requests",
                newName: "IX_rental_requests_ChartererId");

            migrationBuilder.RenameIndex(
                name: "IX_laytime_record_ContractId",
                table: "laytime_records",
                newName: "IX_laytime_records_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_cargo_ContractId_CargoTypeId",
                table: "contract_cargos",
                newName: "IX_contract_cargos_ContractId_CargoTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_cargo_CargoTypeId",
                table: "contract_cargos",
                newName: "IX_contract_cargos_CargoTypeId");

            migrationBuilder.AddColumn<decimal>(
                name: "AgreedTotalPrice",
                table: "rental_contract",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_vessel_categories",
                table: "vessel_categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rental_requests",
                table: "rental_requests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ports",
                table: "ports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_laytime_records",
                table: "laytime_records",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contract_cargos",
                table: "contract_cargos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cargo_types",
                table: "cargo_types",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_cargos_cargo_types_CargoTypeId",
                table: "contract_cargos",
                column: "CargoTypeId",
                principalTable: "cargo_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_rental_requests_users_ChartererId",
                table: "rental_requests",
                column: "ChartererId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_requests_vessels_VesselId",
                table: "rental_requests",
                column: "VesselId",
                principalTable: "vessels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vessels_ports_PortId",
                table: "vessels",
                column: "PortId",
                principalTable: "ports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vessels_users_OwnerId",
                table: "vessels",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vessels_vessel_categories_CategoryId",
                table: "vessels",
                column: "CategoryId",
                principalTable: "vessel_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contract_cargos_cargo_types_CargoTypeId",
                table: "contract_cargos");

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

            migrationBuilder.DropForeignKey(
                name: "FK_rental_requests_users_ChartererId",
                table: "rental_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_rental_requests_vessels_VesselId",
                table: "rental_requests");

            migrationBuilder.DropForeignKey(
                name: "FK_vessels_ports_PortId",
                table: "vessels");

            migrationBuilder.DropForeignKey(
                name: "FK_vessels_users_OwnerId",
                table: "vessels");

            migrationBuilder.DropForeignKey(
                name: "FK_vessels_vessel_categories_CategoryId",
                table: "vessels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vessel_categories",
                table: "vessel_categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rental_requests",
                table: "rental_requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ports",
                table: "ports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_laytime_records",
                table: "laytime_records");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contract_cargos",
                table: "contract_cargos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cargo_types",
                table: "cargo_types");

            migrationBuilder.DropColumn(
                name: "AgreedTotalPrice",
                table: "rental_contract");

            migrationBuilder.RenameTable(
                name: "vessel_categories",
                newName: "vessel_category");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "rental_requests",
                newName: "rental_request");

            migrationBuilder.RenameTable(
                name: "ports",
                newName: "port");

            migrationBuilder.RenameTable(
                name: "laytime_records",
                newName: "laytime_record");

            migrationBuilder.RenameTable(
                name: "contract_cargos",
                newName: "contract_cargo");

            migrationBuilder.RenameTable(
                name: "cargo_types",
                newName: "cargo_type");

            migrationBuilder.RenameIndex(
                name: "IX_users_Email",
                table: "user",
                newName: "IX_user_Email");

            migrationBuilder.RenameIndex(
                name: "IX_rental_requests_VesselId",
                table: "rental_request",
                newName: "IX_rental_request_VesselId");

            migrationBuilder.RenameIndex(
                name: "IX_rental_requests_ChartererId",
                table: "rental_request",
                newName: "IX_rental_request_ChartererId");

            migrationBuilder.RenameIndex(
                name: "IX_laytime_records_ContractId",
                table: "laytime_record",
                newName: "IX_laytime_record_ContractId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_cargos_ContractId_CargoTypeId",
                table: "contract_cargo",
                newName: "IX_contract_cargo_ContractId_CargoTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_contract_cargos_CargoTypeId",
                table: "contract_cargo",
                newName: "IX_contract_cargo_CargoTypeId");

            migrationBuilder.AddColumn<int>(
                name: "LaytimeHours",
                table: "rental_contract",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "rental_contract",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_vessel_category",
                table: "vessel_category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rental_request",
                table: "rental_request",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_port",
                table: "port",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_laytime_record",
                table: "laytime_record",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contract_cargo",
                table: "contract_cargo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cargo_type",
                table: "cargo_type",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_contract_cargo_cargo_type_CargoTypeId",
                table: "contract_cargo",
                column: "CargoTypeId",
                principalTable: "cargo_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contract_cargo_rental_contract_ContractId",
                table: "contract_cargo",
                column: "ContractId",
                principalTable: "rental_contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_laytime_record_rental_contract_ContractId",
                table: "laytime_record",
                column: "ContractId",
                principalTable: "rental_contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_contract_rental_request_RentalRequestId",
                table: "rental_contract",
                column: "RentalRequestId",
                principalTable: "rental_request",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_contract_user_OwnerId",
                table: "rental_contract",
                column: "OwnerId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_request_user_ChartererId",
                table: "rental_request",
                column: "ChartererId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rental_request_vessels_VesselId",
                table: "rental_request",
                column: "VesselId",
                principalTable: "vessels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vessels_port_PortId",
                table: "vessels",
                column: "PortId",
                principalTable: "port",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vessels_user_OwnerId",
                table: "vessels",
                column: "OwnerId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vessels_vessel_category_CategoryId",
                table: "vessels",
                column: "CategoryId",
                principalTable: "vessel_category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
