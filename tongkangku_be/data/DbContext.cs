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
		//index email
		modelBuilder.entity<User>()
			.HasIndex(u => u.email)
			.IsUnique();

		modelBuilder.Entity<RentalContract>()
			.HasMany(rc => rc.LaytimeRecords)
			.WithOne(lr => lr.RentalContract)
			.HasForeignKey(lr => lr.rentalContractId)
			.OnDelete(DeleteBehavior.Cascade);



	}

}