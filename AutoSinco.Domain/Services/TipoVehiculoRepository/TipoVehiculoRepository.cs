using AutoSinco.Domain.Contracts.TipoVehiculoRepository;
using AutoSinco.Infraestructure;
using AutoSinco.Shared.GeneralDTO;

namespace AutoSinco.Domain.Services.TipoVehiculoRepository
{
    public class TipoVehiculoRepository : ITipoVehiculoRepository
    {
        private readonly DBContext _context;

        public TipoVehiculoRepository(DBContext context)
        {
            _context = context;
        }

        public RespuestaDto ObtenerTiposVehiculo()
        {
            try
            {
                var tiposVehiculo = _context.TipoVehiculos
                    .Select(t => new
                    {
                        t.IdTipoVehiculo,
                        t.Nombre,
                        t.Descripcion,
                        t.LimiteInventario
                    })
                    .ToList();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Tipos de vehículo obtenidos exitosamente",
                    Resultado = tiposVehiculo
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ObtenerTipoVehiculoPorId(int idTipoVehiculo)
        {
            try
            {
                var tipoVehiculo = _context.TipoVehiculos
                    .Where(t => t.IdTipoVehiculo == idTipoVehiculo)
                    .Select(t => new
                    {
                        t.IdTipoVehiculo,
                        t.Nombre,
                        t.Descripcion,
                        t.LimiteInventario
                    })
                    .FirstOrDefault();

                if (tipoVehiculo == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Tipo de vehículo no encontrado",
                        "El tipo de vehículo especificado no existe");
                }

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Tipo de vehículo obtenido exitosamente",
                    Resultado = tipoVehiculo
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public bool ExisteTipoVehiculo(int idTipoVehiculo)
        {
            return _context.TipoVehiculos.Any(t => t.IdTipoVehiculo == idTipoVehiculo);
        }

        public int ObtenerLimiteInventario(int idTipoVehiculo)
        {
            return _context.TipoVehiculos
                .Where(t => t.IdTipoVehiculo == idTipoVehiculo)
                .Select(t => t.LimiteInventario)
                .FirstOrDefault();
        }
    }
}
