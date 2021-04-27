using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<Ratings> Ratings { get; set; }

        public DbSet<Comments> Comments { get; set; }

        public DbSet<Replies> Replies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            base.OnModelCreating(builder);
            builder.Entity<Complaint>()
             .HasOne(e => e.Category)
             .WithMany(e => e.Complaints)
             .HasForeignKey(e => e.CategoryId)
             .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Ratings>()
                .HasOne(e => e.Complaint)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.ComplaintId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Comments>()
             .HasOne(e => e.Complaints)
             .WithMany(e => e.Comments)
             .HasForeignKey(e => e.ComplaintId)
             .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<Replies>()
             .HasOne(e => e.Comments)
             .WithMany(e => e.Replies)
             .HasForeignKey(e => e.CommentId)
             .OnDelete(DeleteBehavior.ClientCascade);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)

        {
            var entries = ChangeTracker
                .Entries()
                .Where(entry => entry.Entity is BaseEntity && (
                        entry.State == EntityState.Added
                        || entry.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).Updated_at = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).Created_at = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}