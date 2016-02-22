using Microsoft.AspNet.Identity.EntityFramework;
using SAP.DAL.DbContext;
using SAP.DAL.Tables;
using System;
using System.Data.Entity;

namespace SAP.DAL.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //tabele -- zwykle
        public DbSet<ApplicationRole> Role { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<Compilers> Compilers { get; set; }
        public DbSet<FinalPhaseScores> FinalPhaseScores { get; set; }
        public DbSet<Phase> Phase { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TasksTestData> TasksTestData { get; set; }
        public DbSet<TournamentUsers> TournamentUsers { get; set; }

        //tabele -- historyczne
        public DbSet<HistoryFinalPhaseScores> HistoryFinalPhaseScores { get; set; }
        public DbSet<HistoryScores> HistoryScores { get; set; }
        public DbSet<HistoryTournamentUsers> HistoryTournamentUsers { get; set; }

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

            modelBuilder.Entity<Tournament>().HasKey(x => x.Id);

            modelBuilder.Entity<Phase>().HasKey(x => x.Id);
            modelBuilder.Entity<Phase>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Tasks>().HasKey(x => x.Id);
            modelBuilder.Entity<Tasks>().HasRequired(x => x.Phase).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TasksTestData>().HasKey(x => x.Id);
            modelBuilder.Entity<TasksTestData>().HasRequired(x => x.Task).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TournamentUsers>().HasKey(x => x.Id);
            modelBuilder.Entity<TournamentUsers>().HasRequired(x => x.Phase).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TournamentUsers>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TournamentUsers>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Compilers>().HasKey(x => x.Id);

            modelBuilder.Entity<FinalPhaseScores>().HasKey(x => x.Id);
            modelBuilder.Entity<FinalPhaseScores>().HasRequired(x => x.Phase).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<FinalPhaseScores>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<FinalPhaseScores>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Scores>().HasKey(x => x.Id);
            modelBuilder.Entity<Scores>().HasRequired(x => x.Task).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Scores>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Scores>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Scores>().HasRequired(x => x.Compiler).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoryFinalPhaseScores>().HasKey(x => x.Id);
            modelBuilder.Entity<HistoryScores>().HasKey(x => x.Id);
            modelBuilder.Entity<HistoryTournamentUsers>().HasKey(x => x.Id);
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