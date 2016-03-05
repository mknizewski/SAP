using Microsoft.AspNet.Identity.EntityFramework;
using SAP.DAL.DbContext;
using SAP.DAL.Tables;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SAP.DAL.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //tabele -- zwykle
        public DbSet<ApplicationRole> Role { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<Compilers> Compilers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FinalPhaseScores> FinalPhaseScores { get; set; }
        public DbSet<Phase> Phase { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TasksTestData> TasksTestData { get; set; }
        public DbSet<TournamentUsers> TournamentUsers { get; set; }
        public DbSet<UsersSchools> UsersSchools { get; set; }
        public DbSet<UsersCounselor> UsersCounselor { get; set; }

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

            modelBuilder.Entity<Contact>().HasKey(x => x.Id);

            modelBuilder.Entity<Phase>().HasKey(x => x.Id);
            modelBuilder.Entity<Phase>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<UsersCounselor>().HasKey(x => x.Id);
            modelBuilder.Entity<UsersCounselor>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<UsersSchools>().HasKey(x => x.Id);
            modelBuilder.Entity<UsersSchools>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

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

            ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
            ApplicationUserManager _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            success = _roleManager.CreateRole(_roleManager, "Root", "Super administrator systemu");
            if (!success == true) return success;

            success = _roleManager.CreateRole(_roleManager, "Admin", "Globalnie zarządza systemem");
            if (!success == true) return success;

            success = _roleManager.CreateRole(_roleManager, "User", "Uczestnik konkursu");
            if (!success) return success;

            //tworzenie konta superusera
            ApplicationUser root = new ApplicationUser
            {
                FirstName = "Administrator",
                LastName = "Administrator",
                UserName = "admin@sap.pl",
                Email = "admin@sap.pl",
                EmailConfirmed = true            
            };

             _userManager.CreateAsync(root, "Admin123!");
             _userManager.AddToRoleAsync(root.Id, "Root");
             _userManager.AddToRoleAsync(root.Id, "Admin");

            //deafultowa inicjalizacja sciezek kompilatora
            context.Compilers.Add(new Tables.Compilers
            {
                CompilerName = "C",
                SystemId = 0,
                FullPath = @"F:\Programy\MiniGW\bin\gcc.exe"
            });

            context.Compilers.Add(new Tables.Compilers
            {
                CompilerName = "C++",
                SystemId = 1,
                FullPath = @"F:\Programy\MiniGW\bin\g++.exe"
            });

            context.Compilers.Add(new Tables.Compilers
            {
                CompilerName = "Java",
                SystemId = 2,
                FullPath = @"C:\Program Files\Java\jdk1.8.0_65\bin\javac.exe"
            });

            context.Compilers.Add(new Tables.Compilers
            {
                CompilerName = "Pascal",
                SystemId = 3,
                FullPath = @"C:\FPC\3.0.0\bin\i386-win32\fpc.exe"
            });

            context.SaveChanges();
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