using ArquitecturaDemo.Shared.Models;

namespace ArquitecturaDemo.Tests
{
    public class UsuariosTests
    {
        [Fact]
        public async Task CrearUsuario_SinRepetir()
        {
            //Arrange
            var controller = ControllerConfig.GetInmemoryUsuariosController();
            var model = new UsuarioDto()
            {
                Codigo = "1234567",
                Password = "adfñl",
                CorreoElectronico = "ksrcdubon@gmail.com",
                Nombre = "Kristo Silver Rodolfo",
                Apellido = "Calel Dubón"
            };

            //Act
            var response = await controller.CreateAsync(model);

            //Assert
            Assert.IsType<CreatedAtActionResult>(response as CreatedAtActionResult);
        }

        [Fact]
        public async Task CrearUsuario_Repetido()
        {
            //Arrange
            var controller = ControllerConfig.GetInmemoryUsuariosController();
            var model = new UsuarioDto()
            {
                Codigo = "1234",
                Password = "adfñl",
                CorreoElectronico = "kristo@gmail.com",
                Nombre = "Kristo",
                Apellido = "Calel"
            };

            //Act
            var response = await controller.CreateAsync(model);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response as BadRequestObjectResult);
        }

        [Fact]
        public async Task ObtenerUsuarios_Correcto()
        {
            //Arrange
            var controller = ControllerConfig.GetInmemoryUsuariosController();

            //Act
            var response = await controller.GetAllAsync();
            var result = response as ObjectResult;
            var responseDto = result?.Value as ResponseDto<object>;
            var list = responseDto?.Value as List<UsuarioDto>;

            //Assert
            Assert.IsType<OkObjectResult>(response as OkObjectResult);
            Assert.NotNull(responseDto);
            Assert.NotNull(list);
            Assert.Equal(3, list?.Count);
        }

        [Fact]
        public async Task ObtenerUsuarios_Incorrecto()
        {
            //Arrange
            var controller = ControllerConfig.GetInmemoryUsuariosController(true);

            //Act
            var response = await controller.GetAllAsync();

            //Assert
            Assert.IsType<NotFoundObjectResult>(response as NotFoundObjectResult);
        }

        [Fact]
        public async Task ObtenerUsuario_Correcto()
        {
            //Arrange
            var controller = ControllerConfig.GetInmemoryUsuariosController();

            //Act
            var response = await controller.GetByIdAsync(1);
            var result = response as ObjectResult;
            var responseDto = result?.Value as ResponseDto<object>;
            var registro = responseDto?.Value as UsuarioDto;

            //Assert
            Assert.IsType<OkObjectResult>(response as OkObjectResult);
            Assert.NotNull(responseDto);
            Assert.NotNull(registro);
            Assert.Equal("1234", registro?.Codigo);
            Assert.Equal("kristo@gmail.com", registro?.CorreoElectronico);
        }

        [Fact]
        public async Task ObtenerUsuario_Incorrecto()
        {
            //Arrange
            var controller = ControllerConfig.GetInmemoryUsuariosController();

            //Act
            var response = await controller.GetByIdAsync(5);

            //Assert
            Assert.IsType<NotFoundObjectResult>(response as NotFoundObjectResult);
        }

        [Fact]
        public async Task ActualizarUsuario_SinExistencia()
        {
            //Arrange
            var controller = ControllerConfig.GetInmemoryUsuariosController();
            var model = new UsuarioDto()
            {
                Codigo = "1234",
                Password = "prueba de password",
                CorreoElectronico = "kristo@gmail.com",
                Nombre = "PRUEBA DE NOMBRE",
                Apellido = "PRUEBA DE APELLIDO"
            };

            //Act
            var actResult = await controller.UpdateAsync(model);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actResult as BadRequestObjectResult);
        }

        [Fact]
        public async Task EliminarUsuario_Correcto()
        {
            //Arrange
            var controller = ControllerConfig.GetInmemoryUsuariosController();

            //Act
            var response = await controller.DeleteByIdAsync(1);

            //Assert
            Assert.IsType<OkObjectResult>(response as OkObjectResult);
        }
    }
}