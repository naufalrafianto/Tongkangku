using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tongkangku_be.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cargo_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargo_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: true),
                    Province = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vessel_categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vessel_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vessels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    PortId = table.Column<Guid>(type: "uuid", nullable: false),
                    CapacityFeed = table.Column<int>(type: "integer", nullable: false),
                    DwtCapacity = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    RatePerDay = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vessels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vessels_ports_PortId",
                        column: x => x.PortId,
                        principalTable: "ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vessels_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vessels_vessel_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "vessel_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rental_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VesselId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChartererId = table.Column<Guid>(type: "uuid", nullable: false),
                    CharterType = table.Column<int>(type: "integer", nullable: false),
                    LoadingPortId = table.Column<Guid>(type: "uuid", nullable: false),
                    DischargingPortId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlanDay = table.Column<int>(type: "integer", nullable: false),
                    BaseHirePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    DurationMultiplier = table.Column<decimal>(type: "numeric", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalEstimatedPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TargetMargin = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rental_requests_ports_DischargingPortId",
                        column: x => x.DischargingPortId,
                        principalTable: "ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rental_requests_ports_LoadingPortId",
                        column: x => x.LoadingPortId,
                        principalTable: "ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rental_requests_users_ChartererId",
                        column: x => x.ChartererId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rental_requests_vessels_VesselId",
                        column: x => x.VesselId,
                        principalTable: "vessels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vessel_docs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VesselId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentType = table.Column<string>(type: "text", nullable: true),
                    DocsName = table.Column<string>(type: "text", nullable: true),
                    DocsNum = table.Column<string>(type: "text", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vessel_docs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vessel_docs_vessels_VesselId",
                        column: x => x.VesselId,
                        principalTable: "vessels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rental_contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractNum = table.Column<string>(type: "text", nullable: false),
                    RentalRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DemurrageRate = table.Column<decimal>(type: "numeric", nullable: false),
                    DespatchRate = table.Column<decimal>(type: "numeric", nullable: false),
                    AgreedRatePerDay = table.Column<decimal>(type: "numeric", nullable: false),
                    AgreedHireAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    AgreedBunkerAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    AgreedOtherCharges = table.Column<decimal>(type: "numeric", nullable: false),
                    AgreedTotalPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rental_contracts_rental_requests_RentalRequestId",
                        column: x => x.RentalRequestId,
                        principalTable: "rental_requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rental_contracts_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rental_cost_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    CostType = table.Column<int>(type: "integer", nullable: false),
                    Bearer = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_cost_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rental_cost_items_rental_requests_RentalRequestId",
                        column: x => x.RentalRequestId,
                        principalTable: "rental_requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rental_offers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    RatePerDay = table.Column<decimal>(type: "numeric", nullable: false),
                    HireAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BunkerAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    OtherCharges = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rental_offers_rental_requests_RentalRequestId",
                        column: x => x.RentalRequestId,
                        principalTable: "rental_requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rental_offers_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rental_request_cargos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    CargoTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rental_request_cargos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rental_request_cargos_cargo_types_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "cargo_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rental_request_cargos_rental_requests_RentalRequestId",
                        column: x => x.RentalRequestId,
                        principalTable: "rental_requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contract_cargos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractId = table.Column<Guid>(type: "uuid", nullable: false),
                    CargoTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CargoName = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    FreightRatePerTon = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contract_cargos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contract_cargos_cargo_types_CargoTypeId",
                        column: x => x.CargoTypeId,
                        principalTable: "cargo_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contract_cargos_rental_contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "rental_contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "laytime_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationType = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LaytimeHours = table.Column<int>(type: "integer", nullable: false),
                    ActualDurationHours = table.Column<int>(type: "integer", nullable: false),
                    OvertimeHours = table.Column<int>(type: "integer", nullable: false),
                    SavedHours = table.Column<int>(type: "integer", nullable: false),
                    DemurrageRate = table.Column<decimal>(type: "numeric", nullable: false),
                    DemurrageAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    DespatchRate = table.Column<decimal>(type: "numeric", nullable: false),
                    DespatchAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    NetLaytimeAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_laytime_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_laytime_records_rental_contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "rental_contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contract_cargos_CargoTypeId",
                table: "contract_cargos",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_contract_cargos_ContractId_CargoTypeId",
                table: "contract_cargos",
                columns: new[] { "ContractId", "CargoTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_laytime_records_ContractId",
                table: "laytime_records",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_contracts_OwnerId",
                table: "rental_contracts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_contracts_RentalRequestId",
                table: "rental_contracts",
                column: "RentalRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rental_cost_items_RentalRequestId",
                table: "rental_cost_items",
                column: "RentalRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_offers_OwnerId",
                table: "rental_offers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_offers_RentalRequestId",
                table: "rental_offers",
                column: "RentalRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_request_cargos_CargoTypeId",
                table: "rental_request_cargos",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_request_cargos_RentalRequestId_CargoTypeId",
                table: "rental_request_cargos",
                columns: new[] { "RentalRequestId", "CargoTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rental_requests_ChartererId",
                table: "rental_requests",
                column: "ChartererId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_requests_DischargingPortId",
                table: "rental_requests",
                column: "DischargingPortId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_requests_LoadingPortId",
                table: "rental_requests",
                column: "LoadingPortId");

            migrationBuilder.CreateIndex(
                name: "IX_rental_requests_VesselId",
                table: "rental_requests",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vessel_docs_VesselId",
                table: "vessel_docs",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_vessels_CategoryId",
                table: "vessels",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_vessels_OwnerId",
                table: "vessels",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_vessels_PortId",
                table: "vessels",
                column: "PortId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contract_cargos");

            migrationBuilder.DropTable(
                name: "laytime_records");

            migrationBuilder.DropTable(
                name: "rental_cost_items");

            migrationBuilder.DropTable(
                name: "rental_offers");

            migrationBuilder.DropTable(
                name: "rental_request_cargos");

            migrationBuilder.DropTable(
                name: "vessel_docs");

            migrationBuilder.DropTable(
                name: "rental_contracts");

            migrationBuilder.DropTable(
                name: "cargo_types");

            migrationBuilder.DropTable(
                name: "rental_requests");

            migrationBuilder.DropTable(
                name: "vessels");

            migrationBuilder.DropTable(
                name: "ports");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "vessel_categories");
        }
    }
}
