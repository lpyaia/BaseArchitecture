using Azure;
using BaseArchitecture.Features.WeatherForecasts.GetWeatherForecasts;

namespace BaseArchitecture.Api.Controllers;

public class WeatherForecastController : IMinimalApiController
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/weather-forecast", GetWeatherForecasts)
            .WithName("GetWeatherForecasts")
            .Produces<Response<List<WeatherForecastDto>>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("WeatherForecast");

        //.RequireAuthorization("ApiScope");
    }

    private static IResult GetWeatherForecasts()
    {
        return Results.Ok(new List<WeatherForecastDto>());
    }

    private static IResult GetWeatherForecastById(Guid id)
    {
        return Results.Ok(new WeatherForecastDto(default, default, default, null));
    }

    private static IResult PostWeatherForecast()
    {
        Guid id = new Guid();

        return Results.Created(
            $"weather-forecast/{id}",
            new WeatherForecastDto(default, default, default, null)
        );
    }

    private static IResult DeleteWeatherForecast()
    {
        return Results.NoContent();
    }
}
