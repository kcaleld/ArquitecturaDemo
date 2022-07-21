using ArquitecturaDemo.BLL;
using ArquitecturaDemo.SVL.Controllers.v1;
using Microsoft.Extensions.Logging;
using Moq;

namespace SistemaILP.Test.LibroIVA.Config
{
    public class ControllerConfig
    {
        public static UsersV1Controller GetInmemoryUsuariosController(bool vacio = false)
        {
            //Mock objects
            var logger = Mock.Of<ILogger<GenericRepository<Usuario, UsuarioDto>>>();
            var logger2 = Mock.Of<ILogger<GenericRepository<Rol, RolDto>>>();
            var logger3 = Mock.Of<ILogger<GenericRepository<UsuarioRol, UsuarioRolDto>>>();

            //Instantiated objects
            var context = UsersDbContextInMemory.Get(vacio);
            var validator = new UsuarioDtoValidator(context);
            var validator2 = new RolDtoValidator();
            var validator3 = new UsuarioRolDtoValidator();
            var mapper = new MapTestConfig().Get();
            var mockRegistroRepo2 = new RolesBL(context, mapper, validator2, logger2);
            var mockRegistroRepo3 = new UsuariosRolesBL(context, mapper, validator3, logger3);

            //Controller instance with mock and instantiated objects
            var mockRegistroRepo = new UsuariosBL(context, mapper, validator, logger, mockRegistroRepo2, mockRegistroRepo3);

            return new UsersV1Controller(mockRegistroRepo);
        }
    }
}