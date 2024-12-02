using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.VentaDto;

namespace AutoSinco.Domain.Contracts.VentaRepository
{
    public interface IVentaRepository
    {
        RespuestaDto ObtenerVentas(int pagina, int registrosPorPagina, string? idUsuarioVendedor = null);
        RespuestaDto ObtenerVentaPorId(int idVenta);
        RespuestaDto RegistrarVenta(VentaRegistroDto venta);
    }
}
