using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace task5
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<Version> Versions { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
                .HasKey(m => m.Id);
            modelBuilder.Entity<Version>()
                .HasOne(p => p.Material)
                .WithMany(b => b.Versions)
                .HasForeignKey(k => k.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
