public class DbContext : DbContext
{
	public DbContext(DbContextOptions<DbContext> options) : base(options)
	{
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Vessel> Vessels { get; set; }
    public DbSet<Port> Ports { get; set; }
	public DbSet<VesselCategory> VesselCategories { get; set; }
	public DbSet<VesselDocs> VesselDocs { get; set; }
	public DbSet <RentalContract> RentalContracts { get; set; }
	public DbSet <RentalRequest> RentalRequest { get; set; }
	public DbSet<LaytimeRecord> LaytimeRecords { get; set; }
	public DbSet<CargoType> CargoTypes { get; set; }
	public DbSet <ContractCargo> ContractCargos { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<User>()
				.HasIndex(u => u.email)
				.IsUnique();;

		modelBuilder.Entity<Vessel>()
			.HasOne(v => v.owner)
			.WithMany(u => u.Vessels)
			.HasForeignKey(v => v.ownerId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<RentalRequest>()
			.HasOne(rr => rr.charterer)
			.WithMany(u => u.RentalRequests)
			.HasForeignKey(rr => rr.chartererId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<RentalContract>()
			.HasOne(rc => rc.owner)
			.WithMany(u => u.OwnerContracts)
			.HasForeignKey(rc => rc.ownerId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<RentalContract>()
			.HasOne(rc => rc.charterer)
			.WithMany(u => u.ChartererContracts)
			.HasForeignKey(rc => rc.chartererId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Vessel>()
			.HasOne(v => v.category)
			.WithMany(vc => vc.Vessels)
			.HasForeignKey(v => v.categoryId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Vessel>()
			.HasOne(v => v.port)
			.WithMany(p => p.Vessels)
			.HasForeignKey(v => v.portId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<VesselDocs>()
			.HasOne(vd => vd.vessel)
			.WithMany(v => v.VesselDocs)
			.HasForeignKey(vd => vd.vesselId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<RentalRequest>()
			.HasOne(rr => rr.vessel)
			.WithMany(v => v.RentalRequests)
			.HasForeignKey(rr => rr.vesselId)
			.OnDelete(DeleteBehavior.Restrict);


		modelBuilder.Entity<RentalContract>()
			.HasOne(rc => rc.vessel)
			.WithMany(v => v.RentalContracts)
			.HasForeignKey(rc => rc.vesselId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<RentalContract>()
			.HasOne(rc => rc.rentalRequest)
			.WithOne(rr => rr.RentalContract)
			.HasForeignKey<RentalContract>(rc => rc.rentalRequestId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<LaytimeRecord>()
			.HasOne(lr => lr.contract)
			.WithMany(rc => rc.LaytimeRecords)
			.HasForeignKey(lr => lr.contractId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<ContractCargo>()
			.HasOne(cc => cc.contract)
			.WithMany(rc => rc.ContractCargos)
			.HasForeignKey(cc => cc.contractId)
			.OnDelete(DeleteBehavior.Cascade);


		modelBuilder.Entity<ContractCargo>()
			.HasOne(cc => cc.cargoType)
			.WithMany(ct => ct.ContractCargos)
			.HasForeignKey(cc => cc.cargoTypeId)
			.OnDelete(DeleteBehavior.Restrict);
	}

}