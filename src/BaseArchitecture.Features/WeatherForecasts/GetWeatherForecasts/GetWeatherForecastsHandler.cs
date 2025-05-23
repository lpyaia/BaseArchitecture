using BaseArchitecture.Infrastructure.Data;
using BaseArchitecture.Infrastructure.Data.Extensions;
using BaseArchitecture.Shared.Responses;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BaseArchitecture.Features.WeatherForecasts.GetWeatherForecasts;

public sealed class GetWeatherForecastsHandler
    : IRequestHandler<GetWeatherForecastsQuery, PagedResponse<WeatherForecastDto>>
{
    private readonly BaseArchitectureDbContext _dbContext;

    public GetWeatherForecastsHandler(BaseArchitectureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PagedResponse<WeatherForecastDto>> Handle(
        GetWeatherForecastsQuery request,
        CancellationToken cancellationToken
    )
    {
        PagedDbResult<Domain.Entities.WeatherForecast> result = await _dbContext
            .WeatherForecasts.AsNoTracking()
            .OrderByDescending(x => x.Date)
            .ToPagedResultAsync(request.Page, request.PageSize, cancellationToken);

        return result.Convert(WeatherForecastDto.FromDomain);
    }
}
