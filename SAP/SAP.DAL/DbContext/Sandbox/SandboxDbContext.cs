using SAP.DAL.Tables.Sandbox;
using System.Data.Entity;

namespace SAP.DAL.DbContext.Sandbox
{
    public class SandboxDbContext : System.Data.Entity.DbContext
    {
        public DbSet<Compilers> Compilers { get; set; }
        public DbSet<Tokens> Tokens { get; set; }

        public SandboxDbContext()
            : base("SandboxDbContext")
        {

        }

        public static SandboxDbContext Create()
        {
            return new SandboxDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Compilers>().HasKey(x => x.Id);
            modelBuilder.Entity<Tokens>().HasKey(x => x.Id);
        }

        public void Seed(SandboxDbContext context)
        {

        }
    }

    public class SandboxDbInicializer : CreateDatabaseIfNotExists<SandboxDbContext>
    {
        protected override void Seed(SandboxDbContext context)
        {
            context.Seed(context);
            base.Seed(context);
        }
    }
}
