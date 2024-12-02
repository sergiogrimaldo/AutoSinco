using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.VehiculoDto;

namespace AutoSinco.Domain.Contracts.VehiculoRepository
{
    public interface IVehiculoRepository
    {
        RespuestaDto ObtenerVehiculos(int? tipoVehiculoId = null, string? estado = null, bool? esNuevo = null);
        RespuestaDto ObtenerVehiculoPorId(int idVehiculo);
        RespuestaDto RegistrarVehiculo(VehiculoRegistroDto vehiculo);
        RespuestaDto ActualizarVehiculo(int idVehiculo, VehiculoActualizacionDto vehiculo);
        RespuestaDto CambiarEstadoVehiculo(int idVehiculo, string estado);
        bool ValidarLimiteInventario(int tipoVehiculoId);
        bool ValidarPrecioUsado(decimal precioUsado, string modelo, int tipoVehiculoId);
    }
}
