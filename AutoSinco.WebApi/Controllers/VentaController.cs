using AutoSinco.Domain.Contracts.VentaRepository;
using AutoSinco.Domain.Contracts;
using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.VentaDto;
using AutoSinco.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AutoSinco.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogAttribute))]
    [ServiceFilter(typeof(AutorizacionJwtAttribute))]
    public class VentaController : ControllerBase
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly ILogRepository _logRepository;

        public VentaController(
            IVentaRepository ventaRepository,
            ILogRepository logRepository)
        {
            _ventaRepository = ventaRepository;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene el listado de ventas paginado
        /// </summary>
        [HttpGet]
        public IActionResult ObtenerVentas(
            [FromQuery] int pagina = 1,
            [FromQuery] int registrosPorPagina = 10,
            [FromQuery] string? idUsuarioVendedor = null)
        {
            try
            {
                var resultado = _ventaRepository.ObtenerVentas(pagina, registrosPorPagina, idUsuarioVendedor);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "ObtenerVentas",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Obtiene una venta específica por su ID
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult ObtenerVenta(int id)
        {
            try
            {
                var resultado = _ventaRepository.ObtenerVentaPorId(id);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    $"ObtenerVenta/{id}",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Registra una nueva venta
        /// </summary>
        [HttpPost]
        [ValidarModelo]
        public IActionResult RegistrarVenta([FromBody] VentaRegistroDto venta)
        {
            try
            {
                var resultado = _ventaRepository.RegistrarVenta(venta);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "RegistrarVenta",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }
    }
}
