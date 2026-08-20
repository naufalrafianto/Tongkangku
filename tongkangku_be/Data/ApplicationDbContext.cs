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
        public DbSet<RentalContract> RentalContracts { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }
        public DbSet<LaytimeRecord> LaytimeRecords { get; set; }
        public DbSet<CargoType> CargoTypes { get; set; }
        public DbSet<ContractCargo> ContractCargos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- User ----------
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ---------- Vessel ----------
            modelBuilder.Entity<Vessel>()
                .HasOne(v => v.Owner)
                .WithMany(u => u.Vessels)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vessel>()
                .HasOne(v => v.Category)
                .WithMany(vc => vc.Vessels)
                .HasForeignKey(v => v.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vessel>()
                .HasOne(v => v.Port)
                .WithMany(p => p.Vessels)
                .HasForeignKey(v => v.PortId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- VesselDocs ----------
            modelBuilder.Entity<VesselDocs>()
                .HasOne(vd => vd.Vessel)
                .WithMany(v => v.VesselDocs)
                .HasForeignKey(vd => vd.VesselId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- RentalRequest ----------
            // Satu-satunya tempat yang menyimpan VesselId & ChartererId untuk
            // sebuah pengajuan sewa. RentalContract TIDAK menduplikasi field ini —
            // lihat catatan di blok RentalContract di bawah.
            modelBuilder.Entity<RentalRequest>()
                .HasOne(rr => rr.Vessel)
                .WithMany(v => v.RentalRequests)
                .HasForeignKey(rr => rr.VesselId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RentalRequest>()
                .HasOne(rr => rr.Charterer)
                .WithMany(u => u.RentalRequests)
                .HasForeignKey(rr => rr.ChartererId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- RentalContract ----------
            // RentalContract sengaja TIDAK punya VesselId/ChartererId sendiri.
            // Vessel & Charterer kontrak diakses lewat:
            //   RentalContract.RentalRequest.VesselId / .ChartererId
            // supaya tidak ada 2 sumber kebenaran yang bisa saling tidak sinkron.
            // OwnerId tetap disimpan di RentalContract sebagai snapshot
            // kepemilikan vessel pada saat kontrak dibuat.
            modelBuilder.Entity<RentalContract>()
                .HasOne(rc => rc.RentalRequest)
                .WithOne(rr => rr.RentalContract)
                .HasForeignKey<RentalContract>(rc => rc.RentalRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RentalContract>()
                .HasOne(rc => rc.Owner)
                .WithMany(u => u.OwnerContracts)
                .HasForeignKey(rc => rc.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- LaytimeRecord ----------
            modelBuilder.Entity<LaytimeRecord>()
                .HasOne(lr => lr.Contract)
                .WithMany(rc => rc.LaytimeRecords)
                .HasForeignKey(lr => lr.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- ContractCargo (pivot RentalContract <-> CargoType) ----------
            modelBuilder.Entity<ContractCargo>()
                .HasOne(cc => cc.Contract)
                .WithMany(rc => rc.ContractCargos)
                .HasForeignKey(cc => cc.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ContractCargo>()
                .HasOne(cc => cc.CargoType)
                .WithMany(ct => ct.ContractCargos)
                .HasForeignKey(cc => cc.CargoTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ContractCargo>()
                .HasIndex(cc => new { cc.ContractId, cc.CargoTypeId })
                .IsUnique();
        }
    }
}