using BaseArchitecture.Api.DTOs.WeatherForecast;
using BaseArchitecture.Features.WeatherForecast.GetWeatherForecasts;
using BaseArchitecture.Infrastructure.Data;
using BaseArchitecture.Infrastructure.Data.Extensions;
using BaseArchitecture.Shared.Responses;
using Mediator;

namespace BaseArchitecture.Features.WeatherForecast;

public sealed class GetWeatherForecastsHandler
    : IRequestHandler<GetWeatherForecastsQuery, PagedResponse<WeatherForecastDto>>
{
    private BaseArchitectureDbContext _dbContext;

    public GetWeatherForecastsHandler(BaseArchitectureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<PagedResponse<WeatherForecastDto>> Handle(
        GetWeatherForecastsQuery request,
        CancellationToken cancellationToken
    )
    {
        PagedResponse<Domain.Entities.WeatherForecast> result = await _dbContext
            .WeatherForecasts.AsQueryable()
            .ToPagedResultAsync(request.Page, request.PageSize, cancellationToken);

        List<WeatherForecastDto> resultDto = result
            .Data.Select(WeatherForecastDto.CreateFromDomainEntity)
            .ToList();

        PagedResponse<WeatherForecastDto> pagedResultDto = new PagedResponse<WeatherForecastDto>(
            resultDto,
            result.TotalCount,
            result.PageNumber,
            result.PageSize
        );

        return pagedResultDto;
    }
}
