using Type = PokemonApi.Entities.Type;

namespace PokemonApi.Models;

public class PokemonDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Type Type { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }
    public int Speed { get; set; }
}