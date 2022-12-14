using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonApi.Context;
using PokemonApi.Entities;
using PokemonApi.Models;
using PokemonApi.Services;
using PokemonApi.Exceptions;
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

    private readonly PokemonDto _badCharmanderDto = new()
    {
        Name = new string('a', 260),
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
    public void Create_SinglePokemonWithBadName_ThrowsEntityBadDtoException()
    {
        Assert.Throws<EntityBadDto>(() => _pokemonService.Create(_badCharmanderDto));
    }

    [Fact]
    public void Update_SinglePokemonWithBadName_ThrowsEntityBadDtoException()
    {
        var createdPokemon = _context.Pokemons.Add(_charmanderEntity);
        _context.SaveChanges();

        Assert.Throws<EntityBadDto>(() => _pokemonService.Update(_badCharmanderDto));
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

        _charmanderToUpdate.Id = createdPokemon.Entity.Id;
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

    [Theory]
    [InlineData(-1)]
    [InlineData(-3)]
    [InlineData(100)]
    public void GetById_InvalidId_ThrowsEntityNotFoundException(int id)
    {
        Assert.Throws<EntityNotFound>(() => _pokemonService.GetById(id));
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(15)]
    public void GetAll_ExistingPokemons_ReturnsPokemonList(int pokemonCount)
    {
        var pokemons = Enumerable.Repeat(new Pokemon()
        {
            Name = "Charmander",
            Type = Type.Fire,
            Attack = 15,
            Defense = 10,
            Health = 15,
            SpecialAttack = 15,
            SpecialDefense = 5,
            Speed = 15
        }, pokemonCount).ToList();
        
        _context.AddRange(pokemons);
        _context.SaveChanges();
        
        Assert.Equal(pokemonCount, _pokemonService.GetAll().Count);
    }
}