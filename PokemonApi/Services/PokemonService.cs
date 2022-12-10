using PokemonApi.Context;

namespace PokemonApi.Services;

public class PokemonService
{
    private readonly DatabaseContext _databaseContext;

    public PokemonService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
}