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

            // Configure the LeaveRequest relationship for the user who made the request
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.User)  // The user who made the request
                .WithMany()
                .HasForeignKey(lr => lr.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // Configure the LeaveRequest relationship for the user who approves the request
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.ApprovedByUser)  // The user who approves the request
                .WithMany()
                .HasForeignKey(lr => lr.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            base.OnModelCreating(modelBuilder);
        }
    }
}


