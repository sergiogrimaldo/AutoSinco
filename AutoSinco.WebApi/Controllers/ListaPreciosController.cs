using AutoSinco.Domain.Contracts.ListaPreciosRepository;
using AutoSinco.Domain.Contracts;
using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.ListaPrecioDto;
using AutoSinco.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AutoSinco.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogAttribute))]

    //[ServiceFilter(typeof(AutorizacionJwtAttribute))]
    public class ListaPreciosController : ControllerBase
    {
        private readonly IListaPreciosRepository _listaPreciosRepository;
        private readonly ILogRepository _logRepository;

        public ListaPreciosController(
            IListaPreciosRepository listaPreciosRepository,
            ILogRepository logRepository)
        {
            _listaPreciosRepository = listaPreciosRepository;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Obtiene la lista de precios paginada
        /// </summary>
        [HttpGet]
        public IActionResult ObtenerListaPrecios(
            [FromQuery] int pagina = 1,
            [FromQuery] int registrosPorPagina = 10,
            [FromQuery] int? tipoVehiculoId = null)
        {
            try
            {
                var resultado = _listaPreciosRepository.ObtenerListaPrecios(pagina, registrosPorPagina, tipoVehiculoId);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "ObtenerListaPrecios",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Obtiene el precio de un modelo específico
        /// </summary>
        [HttpGet("modelo")]
        public IActionResult ObtenerPrecioPorModelo(
            [FromQuery] string modelo,
            [FromQuery] int tipoVehiculoId)
        {
            try
            {
                var resultado = _listaPreciosRepository.ObtenerPrecioPorModelo(modelo, tipoVehiculoId);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "ObtenerPrecioPorModelo",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Registra un nuevo precio en la lista
        /// </summary>
        [HttpPost]
        [ValidarModelo]
        public IActionResult RegistrarPrecio([FromBody] ListaPrecioRegistroDto precio)
        {
            try
            {
                var resultado = _listaPreciosRepository.RegistrarPrecio(precio);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    "RegistrarPrecio",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }

        /// <summary>
        /// Actualiza un precio existente
        /// </summary>
        [HttpPut("{id}")]
        [ValidarModelo]
        public IActionResult ActualizarPrecio(
            int id,
            [FromBody] ListaPrecioActualizacionDto precio)
        {
            try
            {
                var resultado = _listaPreciosRepository.ActualizarPrecio(id, precio);
                return StatusCode(resultado.Exito ? 200 : 400, resultado);
            }
            catch (Exception ex)
            {
                _logRepository.Error(
                    HttpContext.Request.Headers["IdUsuario"],
                    HttpContext.Connection.RemoteIpAddress?.ToString(),
                    $"ActualizarPrecio/{id}",
                    ex.Message
                );
                return StatusCode(500, RespuestaDto.ErrorInterno());
            }
        }
    }
}
