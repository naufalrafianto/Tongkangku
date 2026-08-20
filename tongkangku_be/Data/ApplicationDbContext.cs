using Microsoft.EntityFrameworkCore;
using tongkangku_be.Models;

namespace tongkangku_be.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Vessel> Vessels { get; set; }

        public DbSet<Port> Ports { get; set; }

        public DbSet<VesselCategory> VesselCategories { get; set; }

        public DbSet<VesselDocs> VesselDocs { get; set; }

        public DbSet<RentalRequest> RentalRequests { get; set; }

        public DbSet<RentalRequestCargo> RentalRequestCargos { get; set; }

        public DbSet<RentalOffer> RentalOffers { get; set; }

        public DbSet<RentalCostItem> RentalCostItems { get; set; }

        public DbSet<RentalContract> RentalContracts { get; set; }

        public DbSet<LaytimeRecord> LaytimeRecords { get; set; }

        public DbSet<CargoType> CargoTypes { get; set; }

        public DbSet<ContractCargo> ContractCargos { get; set; }
        public DbSet<RentalOperationalCost> RentalOperationalCosts { get; set; }
        public DbSet<RentalPricingSetting> RentalPricingSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // =========================================================
            // USER
            // =========================================================

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            // =========================================================
            // VESSEL
            // =========================================================

            // User (Owner) 1 ---- * Vessel
            modelBuilder.Entity<Vessel>()
                .HasOne(v => v.Owner)
                .WithMany(u => u.Vessels)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);


            // VesselCategory 1 ---- * Vessel
            modelBuilder.Entity<Vessel>()
                .HasOne(v => v.Category)
                .WithMany(vc => vc.Vessels)
                .HasForeignKey(v => v.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // Port 1 ---- * Vessel
            // Current/Home Port
            modelBuilder.Entity<Vessel>()
                .HasOne(v => v.Port)
                .WithMany(p => p.Vessels)
                .HasForeignKey(v => v.PortId)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================================================
            // VESSEL DOCUMENT
            // =========================================================

            // Vessel 1 ---- * VesselDocs
            modelBuilder.Entity<VesselDocs>()
                .HasOne(vd => vd.Vessel)
                .WithMany(v => v.VesselDocs)
                .HasForeignKey(vd => vd.VesselId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================================================
            // RENTAL REQUEST
            // =========================================================

            // Vessel 1 ---- * RentalRequest
            modelBuilder.Entity<RentalRequest>()
                .HasOne(rr => rr.Vessel)
                .WithMany(v => v.RentalRequests)
                .HasForeignKey(rr => rr.VesselId)
                .OnDelete(DeleteBehavior.Restrict);


            // User (Charterer) 1 ---- * RentalRequest
            modelBuilder.Entity<RentalRequest>()
                .HasOne(rr => rr.Charterer)
                .WithMany(u => u.RentalRequests)
                .HasForeignKey(rr => rr.ChartererId)
                .OnDelete(DeleteBehavior.Restrict);


            // Loading Port 1 ---- * RentalRequest
            modelBuilder.Entity<RentalRequest>()
                .HasOne(rr => rr.LoadingPort)
                .WithMany(p => p.LoadingRentalRequests)
                .HasForeignKey(rr => rr.LoadingPortId)
                .OnDelete(DeleteBehavior.Restrict);


            // Discharging Port 1 ---- * RentalRequest
            modelBuilder.Entity<RentalRequest>()
                .HasOne(rr => rr.DischargingPort)
                .WithMany(p => p.DischargingRentalRequests)
                .HasForeignKey(rr => rr.DischargingPortId)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================================================
            // RENTAL REQUEST CARGO
            // =========================================================

            // RentalRequest 1 ---- * RentalRequestCargo
            modelBuilder.Entity<RentalRequestCargo>()
                .HasOne(rc => rc.RentalRequest)
                .WithMany(rr => rr.Cargos)
                .HasForeignKey(rc => rc.RentalRequestId)
                .OnDelete(DeleteBehavior.Cascade);


            // CargoType 1 ---- * RentalRequestCargo
            modelBuilder.Entity<RentalRequestCargo>()
                .HasOne(rc => rc.CargoType)
                .WithMany(ct => ct.RentalRequestCargos)
                .HasForeignKey(rc => rc.CargoTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            // Satu cargo type tidak boleh duplicate
            // dalam satu rental request.
            modelBuilder.Entity<RentalRequestCargo>()
                .HasIndex(rc => new
                {
                    rc.RentalRequestId,
                    rc.CargoTypeId
                })
                .IsUnique();


            // =========================================================
            // RENTAL COST ITEM
            // =========================================================

            // RentalRequest 1 ---- * RentalCostItem
            modelBuilder.Entity<RentalCostItem>()
                .HasOne(ci => ci.RentalRequest)
                .WithMany(rr => rr.CostItems)
                .HasForeignKey(ci => ci.RentalRequestId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================================================
            // RENTAL OFFER
            // =========================================================

            // RentalRequest 1 ---- * RentalOffer
            modelBuilder.Entity<RentalOffer>()
                .HasOne(ro => ro.RentalRequest)
                .WithMany(rr => rr.Offers)
                .HasForeignKey(ro => ro.RentalRequestId)
                .OnDelete(DeleteBehavior.Cascade);


            // User (Owner) 1 ---- * RentalOffer
            modelBuilder.Entity<RentalOffer>()
                .HasOne(ro => ro.Owner)
                .WithMany(u => u.OwnerOffers)
                .HasForeignKey(ro => ro.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================================================
            // RENTAL CONTRACT
            // =========================================================

            // RentalRequest 1 ---- 1 RentalContract
            //
            // Satu RentalRequest hanya boleh menghasilkan
            // satu contract.
            modelBuilder.Entity<RentalContract>()
                .HasOne(rc => rc.RentalRequest)
                .WithOne(rr => rr.RentalContract)
                .HasForeignKey<RentalContract>(
                    rc => rc.RentalRequestId
                )
                .OnDelete(DeleteBehavior.Restrict);


            // User (Owner) 1 ---- * RentalContract
            modelBuilder.Entity<RentalContract>()
                .HasOne(rc => rc.Owner)
                .WithMany(u => u.OwnerContracts)
                .HasForeignKey(rc => rc.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================================================
            // CONTRACT CARGO
            // =========================================================

            // RentalContract 1 ---- * ContractCargo
            modelBuilder.Entity<ContractCargo>()
                .HasOne(cc => cc.Contract)
                .WithMany(rc => rc.ContractCargos)
                .HasForeignKey(cc => cc.ContractId)
                .OnDelete(DeleteBehavior.Cascade);


            // CargoType 1 ---- * ContractCargo
            modelBuilder.Entity<ContractCargo>()
                .HasOne(cc => cc.CargoType)
                .WithMany(ct => ct.ContractCargos)
                .HasForeignKey(cc => cc.CargoTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            // Satu CargoType hanya satu kali
            // dalam satu contract.
            modelBuilder.Entity<ContractCargo>()
                .HasIndex(cc => new
                {
                    cc.ContractId,
                    cc.CargoTypeId
                })
                .IsUnique();


            // =========================================================
            // LAYTIME
            // =========================================================

            // RentalContract 1 ---- * LaytimeRecord
            modelBuilder.Entity<LaytimeRecord>()
                .HasOne(lr => lr.Contract)
                .WithMany(rc => rc.LaytimeRecords)
                .HasForeignKey(lr => lr.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RentalOperationalCost>(entity =>
            {
                entity.ToTable("rental_operational_costs");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                entity.HasIndex(x => x.CostType).IsUnique(); 
            });

            modelBuilder.Entity<RentalPricingSetting>(entity =>
            {
                entity.ToTable("rental_pricing_settings");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(x => x.ContingencyRate).HasColumnType("decimal(5,4)");
                entity.Property(x => x.TargetMargin).HasColumnType("decimal(5,4)");
                entity.Property(x => x.ShortDurationMultiplier).HasColumnType("decimal(5,2)");
                entity.Property(x => x.MediumDurationMultiplier).HasColumnType("decimal(5,2)");
                entity.Property(x => x.LongDurationMultiplier).HasColumnType("decimal(5,2)");
            });
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            var strategy = Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await Database.BeginTransactionAsync();
                try
                {
                    var result = await action();
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            var strategy = Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await Database.BeginTransactionAsync();
                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}