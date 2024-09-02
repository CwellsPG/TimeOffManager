using Microsoft.EntityFrameworkCore;
using TimeOffManager.Models;

namespace TimeOffManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the User-Role relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            // Configure the User-LeaveBalance relationship
            modelBuilder.Entity<LeaveBalance>()
                .HasOne(lb => lb.User)
                .WithMany(u => u.LeaveBalances)
                .HasForeignKey(lb => lb.UserId);

            // Configure the LeaveRequest entity
            modelBuilder.Entity<LeaveRequest>()
                .HasKey(lr => lr.RequestId); // Ensure RequestId is the primary key

            base.OnModelCreating(modelBuilder);
        }
    }
}


