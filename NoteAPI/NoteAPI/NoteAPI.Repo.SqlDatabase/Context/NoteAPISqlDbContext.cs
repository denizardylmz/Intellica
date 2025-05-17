using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NoteAPI.Repo.SqlDatabase.Core;
using NoteAPI.Repo.SqlDatabase.DTO;

namespace NoteAPI.Repo.SqlDatabase.Context
{
    public partial class NoteAPISqlDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContext;
        public NoteAPISqlDbContext()
        {
        }

        public NoteAPISqlDbContext(DbContextOptions<NoteAPISqlDbContext> options, IHttpContextAccessor httpContext)
            : base(options)
        {
            _httpContext = httpContext;
        }

        public virtual DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=HomeDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var user = _httpContext.HttpContext?
                             .User?
                             .FindFirst(ClaimTypes.NameIdentifier)?
                             .Value
                         ?? "Anonymous";

            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = user;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = user;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
