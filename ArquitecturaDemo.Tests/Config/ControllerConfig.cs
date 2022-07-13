using ArquitecturaDemo.BLL;
using Microsoft.Extensions.Logging;
using Moq;

namespace SistemaILP.Test.LibroIVA.Config
{
    public class ControllerConfig
    {
        public static UsersController GetInmemoryUsuariosController(bool vacio = false)
        {
            //Mock objects
            var logger = Mock.Of<ILogger<GenericRepository<Usuario, UsuarioDto>>>();

            //Instantiated objects
            var context = UsersDbContextInMemory.Get(vacio);
            var validator = new UsuarioDtoValidator(context);
            var mapper = new MapTestConfig().Get();


            //Controller instance with mock and instantiated objects
            var mockRegistroRepo = new UsuariosBL(context, mapper, validator, logger);

            return new UsersController(mockRegistroRepo);
        }
    }
}