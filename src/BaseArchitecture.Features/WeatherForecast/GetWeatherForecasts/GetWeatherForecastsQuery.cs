using BaseArchitecture.Api.DTOs.WeatherForecast;
using BaseArchitecture.Shared.Responses;
using Mediator;

namespace BaseArchitecture.Features.WeatherForecast.GetWeatherForecasts;

public sealed record GetWeatherForecastsQuery(int Page, int PageSize = 10)
    : IRequest<PagedResponse<WeatherForecastDto>> { }
