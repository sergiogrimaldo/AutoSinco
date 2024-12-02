using AutoSinco.Domain.Contracts.ReporteRepository;
using AutoSinco.Infraestructure;
using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.ReporteDtos;
using Microsoft.EntityFrameworkCore;

namespace AutoSinco.Domain.Services.ReporteRepository
{
    public class ReporteRepository : IReporteRepository
    {
        private readonly DBContext _context;

        public ReporteRepository(DBContext context)
        {
            _context = context;
        }

        public RespuestaDto ObtenerValorPorTipoYModelo()
        {
            try
            {
                var resumen = _context.Vehiculos
                    .Include(v => v.TipoVehiculo)
                    .Where(v => v.Estado == "Disponible")
                    .GroupBy(v => new { v.TipoVehiculoId, v.TipoVehiculo.Nombre, v.Modelo })
                    .Select(g => new ResumenVehiculosDto
                    {
                        TipoVehiculoId = g.Key.TipoVehiculoId,
                        TipoVehiculo = g.Key.Nombre,
                        Modelo = g.Key.Modelo,
                        CantidadDisponible = g.Count(),
                        ValorTotal = g.Sum(v => v.Valor),
                        ValorPromedio = g.Average(v => v.Valor)
                    })
                    .OrderBy(r => r.TipoVehiculo)
                    .ThenBy(r => r.Modelo)
                    .ToList();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Reporte generado exitosamente",
                    Resultado = resumen
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ObtenerEstadisticasVentas(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var query = _context.Venta.AsQueryable();

                if (fechaInicio.HasValue)
                    query = query.Where(v => v.FechaVenta >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(v => v.FechaVenta <= fechaFin.Value);

                var ventas = query.Include(v => v.IdVehiculoNavigation)
                    .ThenInclude(v => v.TipoVehiculo)
                    .ToList();

                var totalDias = fechaInicio.HasValue && fechaFin.HasValue
                    ? (fechaFin.Value - fechaInicio.Value).Days + 1
                    : (DateTime.Today - ventas.Min(v => v.FechaVenta!.Value)).Days + 1;

                var ventasPorTipo = ventas
                    .GroupBy(v => v.IdVehiculoNavigation.TipoVehiculo.Nombre)
                    .Select(g => new ResumenVentasPorTipoDto
                    {
                        TipoVehiculo = g.Key,
                        CantidadVendida = g.Count(),
                        ValorTotal = g.Sum(v => v.ValorVenta)
                    })
                    .ToList();

                var estadisticas = new EstadisticasVentasDto
                {
                    TotalVentas = ventas.Count,
                    ValorTotalVentas = ventas.Sum(v => v.ValorVenta),
                    PromedioVentaDiaria = ventas.Count / (decimal)totalDias,
                    VentasPorTipo = ventasPorTipo
                };

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Estadísticas generadas exitosamente",
                    Resultado = estadisticas
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }
    }
}
