using AutoSinco.Shared.GeneralDTO;

namespace AutoSinco.Domain.Contracts.ReporteRepository
{
    public interface IReporteRepository
    {
        RespuestaDto ObtenerValorPorTipoYModelo();
        RespuestaDto ObtenerEstadisticasVentas(DateTime? fechaInicio = null, DateTime? fechaFin = null);
    }
}
