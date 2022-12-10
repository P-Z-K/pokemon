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
}