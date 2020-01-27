using AutoMapper;
using ProAgil.Domain;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDto>();
            CreateMap<Palestrante, PalestranteDto>();
            CreateMap<Lote, LoteDto>();
            CreateMap<RedeSocial, RedeSocialDto>();
        }
    }
}