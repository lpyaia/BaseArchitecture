using Mediator;

namespace BaseArchitecture.Features.WeatherForecasts.PostWeatherForecast;

public sealed record PostWeatherForecastCommand(DateTime Date, int TemperatureC, string? Summary)
    : ICommand<WeatherForecastDto> { }
