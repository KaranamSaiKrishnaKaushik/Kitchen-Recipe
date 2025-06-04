namespace Kitchen_Recipe;

public interface IEndpointMapperFactory
{
    IEnumerable<IEndpointMapper> GetEndpointMappers();
}

public class EndpointMapperFactory : IEndpointMapperFactory
{
    private readonly IEnumerable<IEndpointMapper> _mappers;

    public EndpointMapperFactory(IEnumerable<IEndpointMapper> mappers)
    {
        _mappers = mappers;
    }

    public IEnumerable<IEndpointMapper> GetEndpointMappers() => _mappers;
}