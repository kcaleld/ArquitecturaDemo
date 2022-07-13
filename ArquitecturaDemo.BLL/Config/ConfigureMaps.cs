using AutoMapper;
using ArquitecturaDemo.DAL.Entities;
using ArquitecturaDemo.DTO.Users;

namespace ArquitecturaDemo.BLL.Config
{
    public class ConfigureMaps : Profile
    {
        public ConfigureMaps()
        {
            CreateMap<UsuarioDto, Usuario>()
                .ReverseMap()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"));
            CreateMap<RolDto, Rol>().ReverseMap();
            CreateMap<UsuarioRolDto, UsuarioRol>().ReverseMap();
        }
    }
}