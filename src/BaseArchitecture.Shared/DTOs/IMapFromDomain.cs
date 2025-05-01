namespace BaseArchitecture.Shared.DTOs;

public interface IMapFromDomain<TDomain, TDto>
    where TDto : IMapFromDomain<TDomain, TDto>
{
    static abstract TDto FromDomain(TDomain domain);
}
