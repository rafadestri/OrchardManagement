using Microsoft.EntityFrameworkCore;
using OrchardManagement.Models;

namespace OrchardManagement.Data
{
    public class OrchardContext : DbContext
    {
        public OrchardContext(DbContextOptions<OrchardContext> options) : base(options)
        {

        }

        public DbSet<Species> Species { get; set; }
        public DbSet<Tree> Tree { get; set; }
        public DbSet<TreeGroup> TreeGroup { get; set; }
        public DbSet<TreeGroupTree> TreeGroupTree { get; set; }
        public DbSet<Harvest> Harvest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TreeGroupTree>()
                .HasKey(c => new { c.TreeID, c.TreeGroupID });
        }
    }
}
