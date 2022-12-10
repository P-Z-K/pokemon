using Microsoft.AspNetCore.Mvc;
using PokemonApi.Models;

namespace PokemonApi.Controllers;

[Route("api/pokemon")]
[ApiController]
public class PokemonController : ControllerBase
{
    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return BadRequest();
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        return BadRequest();
    }

    [HttpPost]
    public IActionResult Post([FromBody] PokemonDto dto)
    {
        return BadRequest();
    }

    [HttpPut]
    public IActionResult Put([FromBody] PokemonDto dto)
    {
        return BadRequest();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return BadRequest();
    }
}