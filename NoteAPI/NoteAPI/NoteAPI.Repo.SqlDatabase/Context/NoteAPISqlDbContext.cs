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
        public DbSet<BinanceKline> BinanceKlines { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //    optionsBuilder.UseNpgsql("Host=ep-tight-snowflake-abxnllz4-pooler.eu-west-2.aws.neon.tech;Port=5432;Username=neondb_owner;Password=npg_qVi8t2ubpQDN;Database=neondb;SSL Mode=Require;");
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
