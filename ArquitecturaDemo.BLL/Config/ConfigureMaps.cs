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
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => ClearNombre(src.Nombre)));
            CreateMap<RolDto, Rol>().ReverseMap();
            CreateMap<UsuarioRolDto, UsuarioRol>().ReverseMap();
        }

        private static string ClearNombre(string text) => text.Trim().Replace("$", "");
    }
}