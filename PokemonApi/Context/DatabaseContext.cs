using Microsoft.EntityFrameworkCore;
using PokemonApi.Entities;

namespace PokemonApi.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Pokemon> Pokemons { get; set; }
}