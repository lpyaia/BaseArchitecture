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
            .WithTags("WeatherForecast")
            .RequireAuthorization();

        app.MapPost("/weather-forecasts", PostWeatherForecast)
            .WithName("PostWeatherForecast")
            .Produces<WeatherForecastDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("WeatherForecast");

        app.MapGet("admin", () => "Admin!")
            .WithName("RoleAdmin")
            .Produces(StatusCodes.Status200OK)
            .WithTags("RoleAdmin")
            .RequireAuthorization("RequireAdminRole");

        app.MapGet("user", () => "User!")
            .WithName("RoleUser")
            .Produces(StatusCodes.Status200OK)
            .WithTags("RoleUser")
            .RequireAuthorization("RequireUserRole");

        app.MapGet("admin-user", () => "Admin+User!")
            .WithName("RoleAdminUser")
            .Produces(StatusCodes.Status200OK)
            .WithTags("RoleAdminUser")
            .RequireAuthorization("RequireAdminOrUserRole");

        app.MapGet("it", () => "IT")
            .WithName("OnlyITAllowed")
            .Produces(StatusCodes.Status200OK)
            .WithTags("OnlyITAllowed")
            .RequireAuthorization("OnlyITAllowed");

        app.MapGet("sales", () => "Sales")
            .WithName("OnlySalesAllowed")
            .Produces(StatusCodes.Status200OK)
            .WithTags("OnlySalesAllowed")
            .RequireAuthorization("OnlySalesAllowed");

        app.MapGet("sales-it", () => "Sales or IT")
            .WithName("SalesOrITAllowed")
            .Produces(StatusCodes.Status200OK)
            .WithTags("SalesOrITAllowed")
            .RequireAuthorization("SalesOrITAllowed");

        app.MapGet("sales-and-it", () => "Sales and IT")
            .WithName("SalesAndITAllowed")
            .Produces(StatusCodes.Status200OK)
            .WithTags("SalesAndITAllowed")
            .RequireAuthorization("SalesAndITAllowed");
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
