using System.Threading.Tasks;
using BaseArchitecture.Features.WeatherForecasts;
using BaseArchitecture.Features.WeatherForecasts.GetWeatherForecasts;
using BaseArchitecture.Features.WeatherForecasts.PostWeatherForecast;
using BaseArchitecture.Shared.Responses;
using Mediator;

namespace BaseArchitecture.Api.Controllers;

public class WeatherForecastController : IMinimalApiController
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/weather-forecasts", GetWeatherForecasts)
            .WithName("GetWeatherForecasts")
            .Produces<PagedResponse<WeatherForecastDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("WeatherForecast");

        app.MapPost("/weather-forecasts", PostWeatherForecast)
            .WithName("PostWeatherForecast")
            .Produces<WeatherForecastDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("WeatherForecast");

        //.RequireAuthorization("ApiScope");
    }

    private static async Task<IResult> GetWeatherForecasts(
        IMediator mediator,
        int page,
        int pageSize = 10
    )
    {
        var query = new GetWeatherForecastsQuery(page, pageSize);
        var result = await mediator.Send(query);

        return Results.Ok(result);
    }

    private static async Task<IResult> PostWeatherForecast(
        IMediator mediator,
        PostWeatherForecastCommand command
    )
    {
        var response = await mediator.Send(command);

        return Results.Created($"weather-forecast/{response.Id}", response);
    }
}
