namespace tp5.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cadete, CadeteViewModel>().ReverseMap();
        CreateMap<Pedido, PedidoViewModel>().ReverseMap();
    }
}