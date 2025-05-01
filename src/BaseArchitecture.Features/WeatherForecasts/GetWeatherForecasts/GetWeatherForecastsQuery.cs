using BaseArchitecture.Shared.Responses;
using Mediator;

namespace BaseArchitecture.Features.WeatherForecasts.GetWeatherForecasts;

public sealed record GetWeatherForecastsQuery(int Page, int PageSize = 10)
    : IRequest<PagedResponse<WeatherForecastDto>> { }
