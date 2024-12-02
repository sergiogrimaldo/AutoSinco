using AutoSinco.Domain.Contracts.VehiculoRepository;
using AutoSinco.Domain.Contracts.VentaRepository;
using AutoSinco.Infraestructure;
using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.VentaDto;
using Microsoft.EntityFrameworkCore;

namespace AutoSinco.Domain.Services.VentaRepository
{
    public class VentaRepository : IVentaRepository
    {
        private readonly DBContext _context;
        private readonly IVehiculoRepository _vehiculoRepository;

        public VentaRepository(
            DBContext context,
            IVehiculoRepository vehiculoRepository)
        {
            _context = context;
            _vehiculoRepository = vehiculoRepository;
        }

        public RespuestaDto ObtenerVentas(int pagina, int registrosPorPagina, string? idUsuarioVendedor = null)
        {
            try
            {
                // Utilizamos la funcionalidad de paginación existente
                var ventas = _context.PaginateListDTO<VentaDto, Ventum>(
                    pagina,
                    registrosPorPagina,
                    idUsuarioVendedor != null ? v => v.IdUsuarioVendedor == idUsuarioVendedor : null,
                    v => v.FechaVenta,
                    true
                );

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Ventas obtenidas exitosamente",
                    Resultado = ventas
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ObtenerVentaPorId(int idVenta)
        {
            try
            {
                var venta = _context.Venta
                    .Include(v => v.IdUsuarioVendedorNavigation)
                    .Include(v => v.IdVehiculoNavigation)
                        .ThenInclude(vh => vh.TipoVehiculo)
                    .Where(v => v.IdVenta == idVenta)
                    .Select(v => new VentaDto
                    {
                        IdVenta = v.IdVenta,
                        IdVehiculo = v.IdVehiculo,
                        NombreComprador = v.NombreComprador,
                        DocumentoComprador = v.DocumentoComprador,
                        FechaVenta = v.FechaVenta,
                        ValorVenta = v.ValorVenta,
                        IdUsuarioVendedor = v.IdUsuarioVendedor,
                        NombreVendedor = $"{v.IdUsuarioVendedorNavigation.Nombre} {v.IdUsuarioVendedorNavigation.Apellido}",
                        ModeloVehiculo = v.IdVehiculoNavigation.Modelo,
                        TipoVehiculo = v.IdVehiculoNavigation.TipoVehiculo.Nombre
                    })
                    .FirstOrDefault();

                if (venta == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Venta no encontrada",
                        "La venta especificada no existe");
                }

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Venta obtenida exitosamente",
                    Resultado = venta
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto RegistrarVenta(VentaRegistroDto dto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Verificar que el vehículo existe y está disponible
                var vehiculo = _context.Vehiculos
                    .Where(v => v.IdVehiculo == dto.IdVehiculo && v.Estado == "Disponible")
                    .FirstOrDefault();

                if (vehiculo == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Vehículo no disponible",
                        "El vehículo especificado no existe o no está disponible para la venta");
                }

                // Verificar que el vendedor existe
                var vendedor = _context.Usuarios.Find(dto.IdUsuarioVendedor);
                if (vendedor == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Vendedor no válido",
                        "El vendedor especificado no existe");
                }

                // Registrar la venta
                var venta = new Ventum
                {
                    IdVehiculo = dto.IdVehiculo,
                    NombreComprador = dto.NombreComprador,
                    DocumentoComprador = dto.DocumentoComprador,
                    FechaVenta = DateTime.Now,
                    ValorVenta = dto.ValorVenta,
                    IdUsuarioVendedor = dto.IdUsuarioVendedor
                };

                _context.Venta.Add(venta);

                // Actualizar el estado del vehículo
                vehiculo.Estado = "Vendido";

                _context.SaveChanges();
                transaction.Commit();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Venta registrada exitosamente",
                    Resultado = venta.IdVenta
                };
            }
            catch (Exception)
            {
                transaction.Rollback();
                return RespuestaDto.ErrorInterno();
            }
        }
    }
}
