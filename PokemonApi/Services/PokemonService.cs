using PokemonApi.Context;
using PokemonApi.Entities;
using PokemonApi.Exceptions;
using PokemonApi.Models;

namespace PokemonApi.Services;

public class PokemonService
{
    private readonly DatabaseContext _databaseContext;

    public PokemonService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public bool Create(PokemonDto dto)
    {
        ValidateDto(dto);
        
        _databaseContext.Pokemons.Add(new Pokemon
        {
            Id = dto.Id,
            Name = dto.Name,
            Attack = dto.Attack,
            Defense = dto.Defense,
            Health = dto.Health,
            SpecialAttack = dto.SpecialAttack,
            SpecialDefense = dto.SpecialDefense,
            Speed = dto.Speed
        });
        _databaseContext.SaveChanges();

        return true;
    }

    private void ValidateDto(PokemonDto dto)
    {
        if (dto.Name?.Length > 255)
            throw new EntityBadDto();
    }

    public Pokemon Update(PokemonDto dto)
    {
        ValidateDto(dto);
        
        var foundPokemon = GetById(dto.Id);
        
        foundPokemon.Name = dto.Name;
        foundPokemon.Attack = dto.Attack;
        foundPokemon.Defense = dto.Defense;
        foundPokemon.Health = dto.Health;
        foundPokemon.SpecialAttack = dto.SpecialAttack;
        foundPokemon.SpecialDefense = dto.SpecialDefense;
        foundPokemon.Speed = dto.Speed;

        _databaseContext.Pokemons.Update(foundPokemon);
        _databaseContext.SaveChanges();

        return foundPokemon;
    }

    public Pokemon GetById(int id)
    {
        var foundPokemon = _databaseContext.Pokemons.FirstOrDefault(p => p.Id == id);
        if (foundPokemon == null)
            throw new EntityNotFound();

        return foundPokemon;
    }

    public List<Pokemon> GetAll()
    {
        return _databaseContext.Pokemons.ToList();
    }

    public bool Delete(int id)
    {
        var foundPokemon = GetById(id);

        _databaseContext.Pokemons.Remove(foundPokemon);
        _databaseContext.SaveChanges();
        return true;
    }
}