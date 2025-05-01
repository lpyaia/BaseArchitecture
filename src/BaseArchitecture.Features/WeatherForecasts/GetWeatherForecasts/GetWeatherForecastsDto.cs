using BaseArchitecture.Shared.DTOs;
using BaseArchitecture.Shared.MarkerInterfaces;

namespace BaseArchitecture.Features.WeatherForecasts.GetWeatherForecasts;

public record WeatherForecastDto(Guid Id, DateTime Date, int TemperatureC, string? Summary)
    : IMapFromDomain<Domain.Entities.WeatherForecast, WeatherForecastDto>,
        IDto
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public static WeatherForecastDto FromDomain(Domain.Entities.WeatherForecast domain)
    {
        return new WeatherForecastDto(domain.Id, domain.Date, domain.TemperatureC, domain.Summary);
    }
}
