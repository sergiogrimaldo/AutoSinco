using AutoSinco.Domain.Contracts.TipoVehiculoRepository;
using AutoSinco.Domain.Contracts;
using AutoSinco.Shared.GeneralDTO;
using AutoSinco.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AutoSinco.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogAttribute))]
    [ServiceFilter(typeof(AutorizacionJwtAttribute))]
    public class TipoVehiculoController : ControllerBase
    {
        private readonly ITipoVehiculoRepository _tipoVehiculoRepository;
        private readonly ILogRepository _logRepository;

        public TipoVehiculoController(
            ITipoVehiculoRepository tipoVehiculoRepository,
            ILogRepository logRepository)
        {
            _tipoVehiculoRepository = tipoVehiculoRepository;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene todos los tipos de vehículo disponibles
        /// </summary>
        [HttpGet]
        public IActionResult ObtenerTiposVehiculo()
        {
            try
            {
                var resultado = _tipoVehiculoRepository.ObtenerTiposVehiculo();
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "ObtenerTiposVehiculo",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Obtiene un tipo de vehículo por su ID
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult ObtenerTipoVehiculo(int id)
        {
            try
            {
                var resultado = _tipoVehiculoRepository.ObtenerTipoVehiculoPorId(id);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    $"ObtenerTipoVehiculo/{id}",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }
    }
}
