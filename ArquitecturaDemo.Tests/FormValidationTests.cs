namespace ArquitecturaDemo.Tests
{
    public class FormValidationTests
    {
        [Fact]
        public async Task Form_UsuarioDto_Correcto()
        {
            //Arrange
            var context = UsersDbContextInMemory.Get(false);
            var validator = new UsuarioDtoValidator(context);
            var model = new UsuarioDto()
            {
                Codigo = "1234",
                Password = "adfñl",
                CorreoElectronico = "kristosilver@gmail.com",
                Nombre = "Kristo",
                Apellido = "Calel"
            };

            //Act
            var result = await validator.TestValidateAsync(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
            Assert.Equal(0, result?.Errors.Count);
        }

        [Fact]
        public async Task Form_UsuarioDto_Incorrecto_EmailRepetido()
        {
            //Arrange
            var context = UsersDbContextInMemory.Get(false);
            var validator = new UsuarioDtoValidator(context);
            var model = new UsuarioDto()
            {
                Codigo = "1234",
                Password = "adfñl",
                CorreoElectronico = "kristo@gmail.com",
                Nombre = "Kristo",
                Apellido = "Calel"
            };

            var result = await validator.TestValidateAsync(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x);
            Assert.Equal(1, result?.Errors.Count);
        }

        [Fact]
        public async Task Form_UsuarioDto_Incorrecto_Vacios()
        {
            //Arrange
            var context = UsersDbContextInMemory.Get(false);
            var validator = new UsuarioDtoValidator(context);
            var model = new UsuarioDto()
            {
                Codigo = "",
                Password = "",
                CorreoElectronico = "kristosilver@gmail.com",
                Nombre = "Kristo",
                Apellido = "Calel"
            };

            var result = await validator.TestValidateAsync(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Codigo);
            result.ShouldHaveValidationErrorFor(x => x.Password);
            Assert.Equal(2, result?.Errors.Count);
        }

        [Fact]
        public async Task Form_UsuarioDto_Incorrecto_Todo()
        {
            //Arrange
            var context = UsersDbContextInMemory.Get(false);
            var validator = new UsuarioDtoValidator(context);
            var model = new UsuarioDto()
            {
                Codigo = "",
                Password = "",
                CorreoElectronico = "",
                Nombre = "",
                Apellido = ""
            };

            var result = await validator.TestValidateAsync(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Codigo);
            result.ShouldHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.CorreoElectronico);
            result.ShouldHaveValidationErrorFor(x => x.Nombre);
            result.ShouldHaveValidationErrorFor(x => x.Apellido);
            Assert.Equal(6, result?.Errors.Count);
        }
    }
}
