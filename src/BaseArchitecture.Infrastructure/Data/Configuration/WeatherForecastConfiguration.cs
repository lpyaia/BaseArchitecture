using BaseArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaseArchitecture.Infrastructure.Data.Configuration;

public static class WeatherForecastConfiguration
{
    public static void ConfigureWeatherForecast(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.ToTable("WeatherForecast", "dbo");

            entity.HasKey(e => e.Id).HasName("PK_WeatherForecast");

            entity.Property(e => e.Date).IsRequired();

            entity.Property(e => e.TemperatureC).IsRequired();

            entity.Property(e => e.Summary).HasMaxLength(50).IsRequired(false);
        });
    }
}
