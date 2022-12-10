using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonApi.Entities;

public enum Type
{
    Grass,
    Poison,
    Fire,
    Flying,
    Water,
    Bug,
    Dragon,
    Normal,
    Dark,
    Electric,
    Psychic,
    Ground,
    Ice,
    Steel,
    Fairy,
    Rock,
    Fighting,
    Ghost,
}

public class Pokemon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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