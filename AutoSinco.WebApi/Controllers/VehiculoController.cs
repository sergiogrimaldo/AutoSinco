using AutoSinco.Domain.Contracts.VehiculoRepository;
using AutoSinco.Domain.Contracts;
using AutoSinco.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.VehiculoDto;

namespace AutoSinco.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogAttribute))]
    [ServiceFilter(typeof(AutorizacionJwtAttribute))]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly ILogRepository _logRepository;

        public VehiculoController(
            IVehiculoRepository vehiculoRepository,
            ILogRepository logRepository)
        {
            _vehiculoRepository = vehiculoRepository;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene el listado de vehículos con filtros opcionales
        /// </summary>
        [HttpGet]
        public IActionResult ObtenerVehiculos(
            [FromQuery] int? tipoVehiculoId,
            [FromQuery] string? estado,
            [FromQuery] bool? esNuevo)
        {
            try
            {
                var resultado = _vehiculoRepository.ObtenerVehiculos(tipoVehiculoId, estado, esNuevo);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "ObtenerVehiculos",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Obtiene un vehículo específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult ObtenerVehiculo(int id)
        {
            try
            {
                var resultado = _vehiculoRepository.ObtenerVehiculoPorId(id);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    $"ObtenerVehiculo/{id}",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Registra un nuevo vehículo
        /// </summary>
        [HttpPost]
        [ValidarModelo]
        public IActionResult RegistrarVehiculo([FromBody] VehiculoRegistroDto vehiculo)
        {
            try
            {
                var resultado = _vehiculoRepository.RegistrarVehiculo(vehiculo);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "RegistrarVehiculo",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Actualiza un vehículo existente
        /// </summary>
        [HttpPut("{id}")]
        [ValidarModelo]
        public IActionResult ActualizarVehiculo(int id, [FromBody] VehiculoActualizacionDto vehiculo)
        {
            try
            {
                var resultado = _vehiculoRepository.ActualizarVehiculo(id, vehiculo);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    $"ActualizarVehiculo/{id}",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Cambia el estado de un vehículo
        /// </summary>
        [HttpPatch("{id}/estado")]
        public IActionResult CambiarEstadoVehiculo(int id, [FromBody] string estado)
        {
            try
            {
                var resultado = _vehiculoRepository.CambiarEstadoVehiculo(id, estado);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    $"CambiarEstadoVehiculo/{id}",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }
    }
}
