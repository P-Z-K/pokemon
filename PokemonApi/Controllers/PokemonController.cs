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
        throw new NotImplementedException();
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult Post([FromBody] PokemonDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public IActionResult Put([FromBody] PokemonDto dto)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }
}