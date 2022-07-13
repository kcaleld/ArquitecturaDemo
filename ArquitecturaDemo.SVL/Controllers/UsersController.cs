using ArquitecturaDemo.CBL.v1;
using ArquitecturaDemo.DTO.Users;
using ArquitecturaDemo.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ArquitecturaDemo.SVL.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsuariosBL _usuariosBL;
        private readonly ResponseDto<object> _response;

        public UsersController(IUsuariosBL usuarioBL)
        {
            _usuariosBL = usuarioBL;
            _response = new ResponseDto<object>();
        }

        // GET: api/v1/users
        /// <summary>
        /// Devuelve listado completo de registros de IVA
        /// </summary>
        /// <response code="200">La solicitud se ha realizado correctamente.</response>
        /// <response code="400">No se pudo procesar la solicitud porque tiene un formato incorrecto o es incorrecta.</response>
        /// <response code="404">El recurso solicitado no existe.</response>
        [HttpGet(Name = "getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _usuariosBL.GetAllAsync();

                if (result.Count == 0)
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "No se pudo obtener el listado de usuarios.";
                    _response.ErrorMessage = "No hay ningún usuario.";

                    return NotFound(_response);
                }

                _response.Value = result;
                _response.DisplayMessage = "Listado de usuarios.";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al obtener usuarios";
                _response.ErrorMessage = ex.Message;

                return BadRequest(_response);
            }
        }

        // GET: api/v1/users/5
        /// <summary>
        /// Devuelve un registro de IVA en específico por medio del ID.
        /// </summary>
        /// <param name="id">ID del factor</param>
        /// <response code="200">La solicitud se ha realizado correctamente.</response>
        /// <response code="400">No se pudo procesar la solicitud porque tiene un formato incorrecto o es incorrecta.</response>
        /// <response code="404">El recurso solicitado no existe.</response>
        [HttpGet("{id:int}", Name = "getById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _usuariosBL.GetByIdAsync(id);

                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "Error al obtener el usuario.";
                    _response.ErrorMessage = "No existe el usuario.";

                    return NotFound(_response);
                }

                _response.Value = result;
                _response.DisplayMessage = "Información del usuario.";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al obtener usuario";
                _response.ErrorMessage = ex.Message;

                return BadRequest(_response);
            }
        }

        // POST: api/v1/users
        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        /// <param name="model">Dto del registro desde el cliente</param>
        /// <response code="201">La solicitud tuvo éxito y se creó el recurso.</response>
        /// <response code="400">No se pudo procesar la solicitud porque tiene un formato incorrecto o es incorrecta.</response>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UsuarioDto model)
        {
            try
            {
                var validationResult = await _usuariosBL.ValidateDtoAsync(model);
                var existe = await _usuariosBL.ValidateIfExistAsync(x => x.Codigo.ToLower().Equals(model.Codigo.ToLower()) && x.CorreoElectronico.ToLower().Equals(model.CorreoElectronico.ToLower()));

                if (!validationResult.IsValid || existe)
                {
                    _response.Value = model;
                    _response.IsSuccess = false;

                    if (existe)
                    {
                        _response.DisplayMessage = "No se ha podido crear el usuario.";
                        _response.ErrorMessage = "El usuario ya existe en los registros actuales..";
                    }
                    else
                    {
                        _response.DisplayMessage = "No se ha podido crear el usuario, el modelo es inválido.";
                        _response.ErrorMessage = "Errores del modelo: ";

                        validationResult.Errors.ForEach(x =>
                        {
                            _response.ErrorMessage += $"{x.ErrorMessage} ";
                        });
                    }

                    return BadRequest(_response);
                }

                var result = await _usuariosBL.AddAsync(model);
                _response.Value = result;

                return CreatedAtAction("GetByIdAsync", new { id = result.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al crear el usuario.";
                _response.ErrorMessage = ex.Message;

                return BadRequest(_response);
            }
        }

        // PUT: api/v1/users
        /// <summary>
        /// Actualiza un usuario
        /// </summary>
        /// <param name="model">Dto del registro desde el cliente</param>
        /// <response code="201">La solicitud tuvo éxito y se creó el recurso.</response>
        /// <response code="400">No se pudo procesar la solicitud porque tiene un formato incorrecto o es incorrecta.</response>
        /// <response code="500">El servidor ha encontrado una situación que no sabe cómo manejarla.</response>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UsuarioDto model)
        {
            try
            {
                var validationResult = await _usuariosBL.ValidateDtoAsync(model);

                if (!validationResult.IsValid || model.Id == 0)
                {
                    _response.Value = model;
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "No se ha podido actualizar el usuario, el modelo es inválido.";

                    if (model.Id == 0)
                        _response.ErrorMessage = "Id es requerido.";
                    else
                    {
                        _response.ErrorMessage = "Errores del modelo: ";

                        validationResult.Errors.ForEach(x =>
                        {
                            _response.ErrorMessage += $"{x.ErrorMessage} ";
                        });
                    }

                    return BadRequest(_response);
                }

                var result = await _usuariosBL.Update(model);

                if(result == null)
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "No se pudo actualizar el registro.";
                    _response.ErrorMessage = "Ocurrió un error interno.";

                    return StatusCode(StatusCodes.Status500InternalServerError, _response);
                }

                _response.Value = result;
                _response.DisplayMessage = "El usuario se actualizó correctamente.";

                return CreatedAtAction("GetByIdAsync", new { id = result.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al intentar actualizar el usuario.";
                _response.ErrorMessage = ex.Message;

                return BadRequest(_response);
            }
        }

        // DETELE: api/users/5
        /// <summary>
        /// Elimina un usuario por medio del ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <response code="200">La solicitud se ha realizado correctamente.</response>
        /// <response code="400">No se pudo procesar la solicitud porque tiene un formato incorrecto o es incorrecta.</response>
        /// <response code="404">El recurso solicitado no existe.</response>
        [HttpDelete("{id}", Name = "deleteById")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                var user = await _usuariosBL.GetByIdAsync(id);

                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.DisplayMessage = "No se ha podido eliminar el usuario.";
                    _response.ErrorMessage = "El usuario que intenta eliminar no existe.";

                    return NotFound(_response);
                }

                await _usuariosBL.Remove(user);

                _response.DisplayMessage = "El usuario se ha eliminado con éxito.";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error al elminar el usuario.";
                _response.ErrorMessage = ex.Message;

                return BadRequest(_response);
            }
        }
    }
}
