using AutoSinco.Domain.Contracts.ReporteRepository;
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
    public class ReporteController : ControllerBase
    {
        private readonly IReporteRepository _reporteRepository;
        private readonly ILogRepository _logRepository;

        public ReporteController(
            IReporteRepository reporteRepository,
            ILogRepository logRepository)
        {
            _reporteRepository = reporteRepository;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene el resumen de valor por tipo de vehículo y modelo
        /// </summary>
        [HttpGet("valor-tipo-modelo")]
        public IActionResult ObtenerValorPorTipoYModelo()
        {
            try
            {
                var resultado = _reporteRepository.ObtenerValorPorTipoYModelo();
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "ObtenerValorPorTipoYModelo",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Obtiene estadísticas de ventas
        /// </summary>
        [HttpGet("estadisticas-ventas")]
        public IActionResult ObtenerEstadisticasVentas(
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null)
        {
            try
            {
                var resultado = _reporteRepository.ObtenerEstadisticasVentas(fechaInicio, fechaFin);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "ObtenerEstadisticasVentas",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }
    }
}
