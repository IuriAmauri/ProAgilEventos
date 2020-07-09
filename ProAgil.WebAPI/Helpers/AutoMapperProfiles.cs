using System.Linq;
using AutoMapper;
using ProAgil.Domain;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDto>()
                .ForMember(m => m.Palestrantes, opt => {
                    opt.MapFrom(src => src.PalestranteEventos.Select(s => s.Palestrante).ToList());
                })
                .ReverseMap();
            CreateMap<Palestrante, PalestranteDto>()
                .ForMember(m => m.Eventos, opt => {
                    opt.MapFrom(src => src.PalestranteEventos.Select(s => s.Evento).ToList());
                })
                .ReverseMap();
            CreateMap<Lote, LoteDto>()
                .ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>()
                .ReverseMap();
            CreateMap<User, UserDto>()
                .ReverseMap();
            CreateMap<User, UserLoginDto>()
                .ReverseMap();
        }
    }
}