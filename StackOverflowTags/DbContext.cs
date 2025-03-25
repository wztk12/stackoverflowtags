using Microsoft.EntityFrameworkCore;
using StackOverflowTags.Models;

namespace StackOverflowTags.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}