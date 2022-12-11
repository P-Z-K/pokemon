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

    private readonly PokemonDto _badPokemonDto = new()
    {
        Name = new string('a', 260),
        Type = Type.Grass,
        Attack = 0,
        Defense = 0,
        Health = 2,
        SpecialAttack = 0,
        SpecialDefense = 0,
        Speed = 0
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

    [Theory]
    [InlineData(-1)]
    [InlineData(-3)]
    [InlineData(100)]
    public async Task Get_WithNotExistingId_ReturnsNoContentStatus(int id)
    {
        var response = await _httpClient.GetAsync($"api/pokemon/{id}");
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
        var pokemonJson = JsonConvert.SerializeObject(_badPokemonDto);

        var httpContent = new StringContent(pokemonJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/pokemon/", httpContent);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_WithValidModel_ReturnsOkStatus()
    {
        var pokemonJson = JsonConvert.SerializeObject(_charmanderDto);
        var httpContent = new StringContent(pokemonJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/pokemon/", httpContent);

        PokemonDto _charmanderToUpdate = new()
        {
            Id = _charmanderDto.Id,
            Attack = 35,
            Defense = 5,
            Health = 15,
            SpecialAttack = 20,
            SpecialDefense = 10,
            Speed = 25
        };

        var pokemonJsonUpd = JsonConvert.SerializeObject(_charmanderToUpdate);
        var httpContentUpd = new StringContent(pokemonJson, Encoding.UTF8, "application/json");
        response = await _httpClient.PutAsync("/api/pokemon/", httpContentUpd);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}