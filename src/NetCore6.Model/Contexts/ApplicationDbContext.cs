using Microsoft.EntityFrameworkCore;
using NetCore6.Model.Models;

namespace NetCore6.Model.Contexts
{
    public class ApplicationDbContext: BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<ExamplePerson> ExamplePeople { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}