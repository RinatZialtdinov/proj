using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaterial.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebMaterial.DAL.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<Role> Rolles { get; set; }
        public DbSet<Authority> Authorities { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
                .HasKey(m => m.Id);
            modelBuilder.Entity<Version>()
                .HasOne(p => p.Material)
                .WithMany(b => b.Versions)
                .HasForeignKey(l => l.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RoleAuthority>()
                .HasKey(t => new { t.RoleId, t.AuthorityId });
            modelBuilder.Entity<UserRole>()
                .HasKey(t => new { t.RoleId, t.UserId });
            modelBuilder.Entity<UserRole>()
                .HasOne(d => d.User)
                .WithMany(d => d.UserRoles)
                .HasForeignKey(d => d.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(d => d.Role)
                .WithMany(d => d.UserRoles)
                .HasForeignKey(d => d.RoleId);
            modelBuilder.Entity<RoleAuthority>()
                .HasOne(d => d.Role)
                .WithMany(k => k.RoleAuthorities)
                .HasForeignKey(d => d.RoleId);
            modelBuilder.Entity<RoleAuthority>()
                .HasOne(d => d.Authority)
                .WithMany(k => k.RoleAuthorities)
                .HasForeignKey(d => d.AuthorityId);

            //Role admin = new Role { Name = "admin", Id = 1 };
            //Role reader = new Role { Name = "reader", Id = 2 };
            //Role initiator = new Role { Name = "initiator", Id = 3 };
            //Authority editRole = new Authority { Name = "editRole", Id = 4 };
            //Authority createEssence = new Authority { Name = "createEssence", Id = 5 };
            //Authority readEssence = new Authority { Name = "readEssence", Id = 6 };

            //admin.Authorities.Add(editRole);
            //reader.Authorities.Add(readEssence);
            //initiator.Authorities.Add(createEssence);
            //editRole.Roles.Add(admin);
            //createEssence.Roles.Add(initiator);
            //readEssence.Roles.Add(reader);

            //modelBuilder.Entity<Role>().HasData(new Role[] {admin, reader, initiator });
            //modelBuilder.Entity<Authority>().HasData(new Authority[] { editRole, createEssence, readEssence });
            base.OnModelCreating(modelBuilder);
        }
    }
}
