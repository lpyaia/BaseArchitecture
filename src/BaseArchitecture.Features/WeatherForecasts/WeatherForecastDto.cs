using BaseArchitecture.Shared.DTOs;
using BaseArchitecture.Shared.MarkerInterfaces;

namespace BaseArchitecture.Features.WeatherForecasts;

public record WeatherForecastDto
    : IMapFromDomain<Domain.Entities.WeatherForecast, WeatherForecastDto>,
        IDto
{
    public WeatherForecastDto(Guid id, DateTime date, int temperatureC, string? summary)
    {
        Id = id;
        Date = date;
        TemperatureC = temperatureC;
        Summary = summary;
    }

    public Guid Id { get; init; }

    public DateTime Date { get; init; }

    public int TemperatureC { get; init; }

    public string? Summary { get; init; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public static WeatherForecastDto FromDomain(Domain.Entities.WeatherForecast domain)
    {
        return new WeatherForecastDto(domain.Id, domain.Date, domain.TemperatureC, domain.Summary);
    }
}
