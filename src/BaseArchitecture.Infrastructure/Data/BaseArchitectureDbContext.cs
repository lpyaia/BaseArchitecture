using BaseArchitecture.Domain.Entities;
using BaseArchitecture.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BaseArchitecture.Infrastructure.Data;

public class BaseArchitectureDbContext : DbContext
{
    public BaseArchitectureDbContext(DbContextOptions<BaseArchitectureDbContext> options)
        : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureWeatherForecast();
        base.OnModelCreating(modelBuilder);
    }
}
