using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace MegansBlog.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}