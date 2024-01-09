using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Domain;

namespace Notes.Identity;

public class NotesDbContext(DbContextOptions<NotesDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Note> Notes { get; set; }
    public DbSet<Collection> Collections { get; set; }
}
