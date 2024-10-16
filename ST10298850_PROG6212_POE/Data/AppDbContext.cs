using Microsoft.EntityFrameworkCore;
using ST10298850_PROG6212_POE.Models;

namespace ST10298850_PROG6212_POE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LecturerModel> Lecturers { get; set; } = null!;
        public DbSet<CoordinatorModel> Coordinators { get; set; } = null!;
        public DbSet<AcademicManagerModel> AcademicManagers { get; set; } = null!;
        public DbSet<LecturerClaimModel> Claims { get; set; } = null!;
        public DbSet<DocumentModel> Documents { get; set; } = null!;
        public DbSet<ApprovalModel> Approvals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints here if needed
            modelBuilder.Entity<LecturerModel>()
                .Property(l => l.LecturerId)
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:Identity", "10011, 1");

            modelBuilder.Entity<AcademicManagerModel>()
                .Property(a => a.ManagerId)
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:Identity", "20011, 1");

            modelBuilder.Entity<CoordinatorModel>()
                .Property(c => c.CoordinatorId)
                .ValueGeneratedOnAdd()
                .HasAnnotation("SqlServer:Identity", "30011, 1");

            modelBuilder.Entity<LecturerClaimModel>()
                .HasOne(c => c.Approval)
                .WithOne(a => a.Claim)
                .HasForeignKey<ApprovalModel>(a => a.ClaimId)
                .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete

            modelBuilder.Entity<DocumentModel>()
                .HasOne(d => d.Claim)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete
        }
    }
}
