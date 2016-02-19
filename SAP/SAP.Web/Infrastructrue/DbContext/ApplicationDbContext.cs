using Microsoft.AspNet.Identity.EntityFramework;
using SAP.DAL.Tables;
using SAP.Web.Models;
using System;
using System.Data.Entity;

namespace SAP.Web.Infrastructrue.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //tabele
        public DbSet<ApplicationRole> Role { get; set; }

        public ApplicationDbContext()
            : base("EFDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            base.OnModelCreating(modelBuilder);

            //Defining the keys and relations
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
        }

        public bool Seed(ApplicationDbContext context)
        {
            bool success = false;

            //tworzenie ról w systemie
            ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            success = _roleManager.CreateRole(_roleManager, "Admin", "Globalnie zarządza systemem");
            if (!success == true) return success;

            success = _roleManager.CreateRole(_roleManager, "User", "Uczestnik konkursu");
            if (!success) return success;

            return success;
        }

        /// <summary>
        /// Context Initializer
        /// </summary>
        public class DataBaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }
    }
}