using BaseArchitecture.Shared.MarkerInterfaces;

namespace BaseArchitecture.Domain.Entities;

public class WeatherForecast : IDomainEntity
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
