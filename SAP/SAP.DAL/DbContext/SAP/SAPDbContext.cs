using Microsoft.AspNet.Identity.EntityFramework;
using SAP.DAL.Tables;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SAP.DAL.DbContext.SAP
{
    public class SAPDbContext : IdentityDbContext<ApplicationUser>
    {
        //tabele -- zwykle
        public DbSet<ApplicationRole> Role { get; set; }
        public DbSet<Tournament> Tournament { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<Phase> Phase { get; set; }
        public DbSet<UserSolutions> UserSolutions { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TasksTestData> TasksTestData { get; set; }
        public DbSet<TournamentUsers> TournamentUsers { get; set; }
        public DbSet<UsersSchools> UsersSchools { get; set; }
        public DbSet<UsersCounselor> UsersCounselor { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<ServerSettings> ServerSettings { get; set; }
        public DbSet<Exceptions> Exceptions { get; set; }

        //tabele -- historyczne
        public DbSet<HistoryScores> HistoryScores { get; set; }

        public DbSet<HistoryTournamentUsers> HistoryTournamentUsers { get; set; }

        public SAPDbContext()
            : base("EFDbContext", throwIfV1Schema: false)
        {

        }

        public static SAPDbContext Create()
        {
            return new SAPDbContext();
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

            modelBuilder.Entity<ServerSettings>().HasKey(x => x.Id);

            modelBuilder.Entity<Exceptions>().HasKey(x => x.Id);

            modelBuilder.Entity<Contact>().HasKey(x => x.Id);
            modelBuilder.Entity<News>().HasKey(x => x.Id);

            modelBuilder.Entity<Messages>().HasKey(x => x.Id);
            modelBuilder.Entity<Messages>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

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
            modelBuilder.Entity<TournamentUsers>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TournamentUsers>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Scores>().HasKey(x => x.Id);
            modelBuilder.Entity<Scores>().HasRequired(x => x.Phase).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Scores>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Scores>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSolutions>().HasKey(x => x.Id);
            modelBuilder.Entity<UserSolutions>().HasRequired(x => x.Task).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserSolutions>().HasRequired(x => x.Phase).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserSolutions>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserSolutions>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoryScores>().HasKey(x => x.Id);
            modelBuilder.Entity<HistoryScores>().HasRequired(x => x.Tournament).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<HistoryScores>().HasRequired(x => x.Phase).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<HistoryScores>().HasRequired(x => x.User).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<HistoryTournamentUsers>().HasKey(x => x.Id);
        }

        public async Task<bool> Seed(SAPDbContext context)
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
                EmailConfirmed = true,
            };

            await _userManager.CreateAsync(root, "Admin123!");
            _roleManager.AddUserToRole(_userManager, root.Id, "Root");
            _roleManager.AddUserToRole(_userManager, root.Id, "Admin");

            context.SaveChanges();
            return success;
        }

        /// <summary>
        /// Context Initializer
        /// </summary>
        public class DataBaseInitializer : CreateDatabaseIfNotExists<SAPDbContext>
        {
            protected override async void Seed(SAPDbContext context)
            {
                await context.Seed(context);
                base.Seed(context);
            }
        }
    }
}