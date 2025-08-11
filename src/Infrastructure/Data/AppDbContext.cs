using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Publisher> Publishers => Set<Publisher>();
        public DbSet<Purchase> Purchases => Set<Purchase>();
        public DbSet<Bookmark> Bookmarks => Set<Bookmark>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}