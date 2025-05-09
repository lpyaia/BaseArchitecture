using BaseArchitecture.Domain.Entities;
using BaseArchitecture.Infrastructure.Data;
using Mediator;

namespace BaseArchitecture.Features.WeatherForecasts.PostWeatherForecast;

public class PostWeatherForecastHandler
    : ICommandHandler<PostWeatherForecastCommand, WeatherForecastDto>
{
    private readonly BaseArchitectureDbContext _dbContext;

    public PostWeatherForecastHandler(BaseArchitectureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<WeatherForecastDto> Handle(
        PostWeatherForecastCommand command,
        CancellationToken cancellationToken
    )
    {
        var weatherForecast = new WeatherForecast(
            command.Date,
            command.TemperatureC,
            command.Summary
        );

        await _dbContext.AddAsync(weatherForecast);
        await _dbContext.SaveChangesAsync();

        return WeatherForecastDto.FromDomain(weatherForecast);
    }
}
