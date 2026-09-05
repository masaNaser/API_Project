using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Data
{
    // الخطوة الثانية انه نورث من IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Category> Categories { get; set; }
        public DbSet<CatagoryTranslations> CatagoryTranslations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken=default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(e => e.CreatedById).CurrentValue = currentUserId;
                    entry.Property(e => e.CreatedDate).CurrentValue = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                    entry.Property(e => e.UpdatedDate).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
