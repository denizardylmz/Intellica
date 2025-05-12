using Microsoft.EntityFrameworkCore;

using NoteAPI.Repo.SqlDatabase.DTO;

namespace NoteAPI.Repo.SqlDatabase.Context
{
    public partial class NoteAPISqlDbContext : DbContext
    {
        public NoteAPISqlDbContext()
        {
        }

        public NoteAPISqlDbContext(DbContextOptions<NoteAPISqlDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=HomeDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }
    }
}
