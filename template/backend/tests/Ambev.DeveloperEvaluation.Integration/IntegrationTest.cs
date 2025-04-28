using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.ORM;
using System.Net.Http.Headers;

namespace Ambev.DeveloperEvaluation.Integration;

public abstract class IntegrationTest : IDisposable
{
    protected readonly HttpClient _client;
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly IServiceScope _scope;
    protected readonly DefaultContext _dbContext;

    protected IntegrationTest()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddDbContext<DefaultContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });

        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<DefaultContext>();
    }

    protected async Task AuthenticateAsAdmin()
    {
        var token = "your_test_token_here"; // Replace with actual test token generation
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void Dispose()
    {
        _scope.Dispose();
        _client.Dispose();
        _factory.Dispose();
    }
} 