using AutoSinco.Shared.GeneralDTO;

namespace AutoSinco.Domain.Contracts.TipoVehiculoRepository
{
    public interface ITipoVehiculoRepository
    {
        RespuestaDto ObtenerTiposVehiculo();
        RespuestaDto ObtenerTipoVehiculoPorId(int idTipoVehiculo);
        bool ExisteTipoVehiculo(int idTipoVehiculo);
        int ObtenerLimiteInventario(int idTipoVehiculo);
    }
}
