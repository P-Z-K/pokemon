using Microsoft.AspNetCore.Mvc;
using PokemonApi.Context;
using PokemonApi.Models;
using PokemonApi.Services;

namespace PokemonApi.Controllers;

[Route("api/pokemon")]
[ApiController]
public class PokemonController : ControllerBase
{
    private readonly PokemonService _pokemonService;

    public PokemonController(PokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return Ok(_pokemonService.GetAll());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok(_pokemonService.GetById(id));
    }

    [HttpPost]
    public IActionResult Post([FromBody] PokemonDto dto)
    {
        _pokemonService.Create(dto);
        return Ok();
    }

    [HttpPut]
    public IActionResult Put([FromBody] PokemonDto dto)
    {
        return Ok(_pokemonService.Update(dto));
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return Ok(_pokemonService.Delete(id));
    }
}