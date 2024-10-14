using Microsoft.EntityFrameworkCore;
using ST10298850_PROG6212_POE.Models;
namespace ST10298850_PROG6212_POE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        DbSet<LecturerModel> Lecturers { get; set; }
        DbSet<CoordinatorModel> Coordinators { get; set; }
        DbSet<AcademicManagerModel> AcademicManagers { get; set; }
        DbSet<LecturerClaimModel> Claims { get; set; }
        DbSet<DocumentModel> Documents { get; set; }
        DbSet<ApprovalModel> Approvals { get; set; }
    }
}
