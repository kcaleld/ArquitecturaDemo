using ArquitecturaDemo.BLL.v1;
using ArquitecturaDemo.CBL;
using ArquitecturaDemo.CBL.v1;
using ArquitecturaDemo.DAL;
using ArquitecturaDemo.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArquitecturaDemo.BLL.Config
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddBLConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //Database
            //Se registra el contexto para inyección de dependencias
            services.AddDbContext<UsersContext>(options => options.UseSqlServer(configuration.GetConnectionString(Const.DbUsersConnection)));
            
            //Mapping
            //Se registra el perfil de mapeo de DTOs
            //No se utiliza una instancia ya que eso relentizaría el inicio y el mapeo de objetos
            services.AddAutoMapper(typeof(ConfigureMaps));

            //Repositories
            //Se registra el repositorio genérico para todas las entidades
            //Recibe dos parámetros TEntity que equivale a una entidad de la base de datos y TOutput que equivale a su DTO par ser mapeado
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            //Se registran los respositorios individuales
            services.AddScoped<IUsuariosBL, UsuariosBL>();
            services.AddScoped<IRolesBL, RolesBL>();
            services.AddScoped<IUsuariosRolesBL, UsuariosRolesBL>();

            return services;
        }
    }
}