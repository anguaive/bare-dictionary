using dictionary_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace dictionary_api.Database;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Phrase> Phrases => Set<Phrase>();
}