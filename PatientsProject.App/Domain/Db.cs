using Microsoft.EntityFrameworkCore;

namespace PatientsProject.App.Domain
{
    public class Db : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity => { entity.HasKey(e => e.Id); });

            modelBuilder.Entity<DoctorPatient>()
                .HasKey(dp => new { dp.DoctorId, dp.PatientId });

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Doctor)
                .WithMany(d => d.DoctorPatients)
                .HasForeignKey(dp => dp.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Patient)
                .WithMany(p => p.DoctorPatients)
                .HasForeignKey(dp => dp.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Branch)
                .WithMany(b => b.Doctors)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}