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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:NoteAPIConnectionString");

            // Enable sensitive data logging
        }
    }
}
