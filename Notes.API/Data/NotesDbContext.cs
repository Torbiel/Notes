using Microsoft.EntityFrameworkCore;
using Notes.Models;

namespace Notes.Data
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This lets the Created and Modified be readonly and be bound to properties of Note when creating db in EF Core Code First
            modelBuilder.Entity<Note>(n => n.Property(p => p.Created));
            modelBuilder.Entity<Note>(n => n.Property(p => p.Modified));
        }
    }
}
