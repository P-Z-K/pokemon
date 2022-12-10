using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PokemonApi.Context;
using PokemonApi.Models;
using Type = PokemonApi.Entities.Type;

namespace IntegrationTests;

public class PokemonControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    
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

    public PokemonControllerTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<DatabaseContext>));
                    services.Remove(dbContextOptions);

                    services.AddDbContext<DatabaseContext>(options =>
                        options.UseInMemoryDatabase("PokemonControllerIntegrationTestsDb"));
                });
            }).CreateClient();
    }

    [Fact]
    public async Task GetAll_WithNoParameters_ReturnsOkStatus()
    {
        var response = await _httpClient.GetAsync("/api/pokemon/all");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_WithNotExistingId_ReturnsNoContentStatus()
    {
        var response = await _httpClient.GetAsync($"api/pokemon/{-1}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_WithExistingId_ReturnsOkStatus()
    {
        var response = await _httpClient.GetAsync($"api/pokemon/{1}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_WithValidModel_ReturnsCreatedStatus()
    {
        var pokemonJson = JsonConvert.SerializeObject(_charmanderDto);
        
        var httpContent = new StringContent(pokemonJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/pokemon/", httpContent);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_WithInvalidName_ReturnsBadRequest()
    {
        var badName = new StringBuilder("a");

        for(var i = 0; i < 260; i++)
        {
            badName.Append("a");
        }

        PokemonDto badPokemonDto = new()
        {
            Name = badName.ToString(),
            Type = Type.Grass,
            Attack = 0,
            Defense = 0,
            Health = 2,
            SpecialAttack = 0,
            SpecialDefense = 0,
            Speed = 0
        };

        var pokemonJson = JsonConvert.SerializeObject(badPokemonDto);

        var httpContent = new StringContent(pokemonJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/pokemon/", httpContent);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}