using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonApi.Context;
using PokemonApi.Entities;
using PokemonApi.Models;
using PokemonApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Type = PokemonApi.Entities.Type;

namespace UnitTests;

public class PokemonServiceUnitTest
{
    private readonly PokemonService _pokemonService;
    private readonly DatabaseContext _context;
    
    private readonly PokemonDto _charmanderDto = new()
    {
        Name = "Charmander",
        Type = Type.Fire,
        Attack = 15,
        Defense = 10,
        Health = 15,
        SpecialAttack = 15,
        SpecialDefense = 5,
        Speed = 15
    };
    
    private readonly PokemonDto _charmanderToUpdate = new()
    {
        Attack = 35,
        Defense = 5,
        Health = 15,
        SpecialAttack = 20,
        SpecialDefense = 10,
        Speed = 25
    };

    
    private readonly Pokemon _charmanderEntity = new()
    {
        Name = "Charmander",
        Type = Type.Fire,
        Attack = 15,
        Defense = 10,
        Health = 15,
        SpecialAttack = 15,
        SpecialDefense = 5,
        Speed = 15
    };

    public PokemonServiceUnitTest()
    {
        var contextOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase("PokemonServiceUnitTestsDb")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        
        _context = new DatabaseContext(contextOptions);
        _pokemonService = new PokemonService(_context);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void Create_SinglePokemon_ReturnsTrue()
    {
        var isSuccess = _pokemonService.Create(_charmanderDto);

        Assert.True(isSuccess);
    }

    [Fact]
    public void Create_SinglePokemonWithBadName_ThrowsValidationException()
    {
        var badName = new StringBuilder("a");

        for (var i = 0; i < 260; i++)
        {
            badName.Append("a");
        }
        PokemonDto badPokemonDto = new()
        {
            Name = badName.ToString(),
            Type = Type.Fire,
            Attack = 15,
            Defense = 10,
            Health = 15,
            SpecialAttack = 15,
            SpecialDefense = 5,
            Speed = 15
        };
        Assert.Throws<ValidationException>(_pokemonService.Create(badPokemonDto));
    }

    [Fact]
    public void Delete_ExistingPokemon_ReturnsTrue()
    {
        var createdPokemon = _context.Pokemons.Add(_charmanderEntity);
        _context.SaveChanges();

        var id = createdPokemon.Entity.Id;
        
        Assert.True(_pokemonService.Delete(id));
        Assert.Null(_context.Pokemons.FirstOrDefault(p => p.Id == id));
    }

    [Fact]
    public void Update_ExistingPokemon_ReturnsUpdatedPokemon()
    {
        var createdPokemon = _context.Pokemons.Add(_charmanderEntity);
        _context.SaveChanges();
        
        var updatedPokemon = _pokemonService.Update(_charmanderToUpdate);
        
        Assert.NotNull(updatedPokemon);
    }
    
    [Fact]
    public void GetById_ExistingPokemon_ReturnsPokemonWithCorrectId()
    {
        var createdPokemon = _context.Pokemons.Add(_charmanderEntity);
        _context.SaveChanges();
        
        
        Assert.NotNull(_pokemonService.GetById(createdPokemon.Entity.Id));
    }
    [Fact]
    public void GetAll_ExistingPokemon_ReturnsPokemonList()
    {
        for(var i = 0; i < 5; i++)
        {
            _context.Pokemons.Add(_charmanderEntity);
        }
        List<Pokemon> pokemons = _pokemonService.GetAll();
        Assert.True(pokemons.Any());
    }
}