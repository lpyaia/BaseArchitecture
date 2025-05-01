namespace BaseArchitecture.Api.DTOs.WeatherForecast;

public record WeatherForecastDto(Guid Id, DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public static WeatherForecastDto CreateFromDomainEntity(
        Domain.Entities.WeatherForecast domainEntity
    )
    {
        return new WeatherForecastDto(
            domainEntity.Id,
            domainEntity.Date,
            domainEntity.TemperatureC,
            domainEntity.Summary
        );
    }
}
