using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.ListaPrecioDto;

namespace AutoSinco.Domain.Contracts.ListaPreciosRepository
{
    public interface IListaPreciosRepository
    {
        RespuestaDto ObtenerListaPrecios(int pagina, int registrosPorPagina, int? tipoVehiculoId = null);
        RespuestaDto ObtenerPrecioPorModelo(string modelo, int tipoVehiculoId);
        RespuestaDto RegistrarPrecio(ListaPrecioRegistroDto precio);
        RespuestaDto ActualizarPrecio(int idListaPrecios, ListaPrecioActualizacionDto precio);
        decimal ObtenerPrecioReferencia(string modelo, int tipoVehiculoId);
    }
}
