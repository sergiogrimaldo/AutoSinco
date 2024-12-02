using AutoSinco.Domain.Contracts.ListaPreciosRepository;
using AutoSinco.Domain.Contracts.TipoVehiculoRepository;
using AutoSinco.Infraestructure;
using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.ListaPrecioDto;

namespace AutoSinco.Domain.Services.ListaPreciosRepository
{
    public class ListaPreciosRepository : IListaPreciosRepository
    {
        private readonly DBContext _context;
        private readonly ITipoVehiculoRepository _tipoVehiculoRepository;

        public ListaPreciosRepository(
            DBContext context,
            ITipoVehiculoRepository tipoVehiculoRepository)
        {
            _context = context;
            _tipoVehiculoRepository = tipoVehiculoRepository;
        }

        public RespuestaDto ObtenerListaPrecios(int pagina, int registrosPorPagina, int? tipoVehiculoId = null)
        {
            try
            {
                // Utilizamos la funcionalidad de paginación existente
                var listaPrecios = _context.PaginateListDTO<ListaPrecioDto, ListaPrecio>(
                    pagina,
                    registrosPorPagina,
                    tipoVehiculoId.HasValue ? lp => lp.TipoVehiculoId == tipoVehiculoId : null,
                    lp => lp.FechaCreacion,
                    true
                );

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Lista de precios obtenida exitosamente",
                    Resultado = listaPrecios
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ObtenerPrecioPorModelo(string modelo, int tipoVehiculoId)
        {
            try
            {
                var precio = _context.ListaPrecios
                    .Where(lp => lp.Modelo == modelo &&
                           lp.TipoVehiculoId == tipoVehiculoId &&
                           lp.Activo == true)
                    .Select(lp => new { lp.Modelo, lp.Precio, lp.FechaCreacion })
                    .FirstOrDefault();

                if (precio == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Precio no encontrado",
                        "No existe un precio activo para el modelo especificado");
                }

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Precio obtenido exitosamente",
                    Resultado = precio
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto RegistrarPrecio(ListaPrecioRegistroDto dto)
        {
            try
            {
                if (!_tipoVehiculoRepository.ExisteTipoVehiculo(dto.TipoVehiculoId))
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Tipo de vehículo inválido",
                        "El tipo de vehículo especificado no existe");
                }

                // Desactivar precios anteriores del mismo modelo y tipo
                var preciosAnteriores = _context.ListaPrecios
                    .Where(lp => lp.Modelo == dto.Modelo &&
                           lp.TipoVehiculoId == dto.TipoVehiculoId &&
                           lp.Activo == true);

                foreach (var precio in preciosAnteriores)
                {
                    precio.Activo = false;
                }

                var nuevoPrecio = new ListaPrecio
                {
                    Modelo = dto.Modelo,
                    Precio = dto.Precio,
                    TipoVehiculoId = dto.TipoVehiculoId,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };

                _context.ListaPrecios.Add(nuevoPrecio);
                _context.SaveChanges();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Precio registrado exitosamente",
                    Resultado = nuevoPrecio.IdListaPrecios
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ActualizarPrecio(int idListaPrecios, ListaPrecioActualizacionDto dto)
        {
            try
            {
                var precio = _context.ListaPrecios.Find(idListaPrecios);
                if (precio == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Precio no encontrado",
                        "El precio especificado no existe");
                }

                precio.Precio = dto.Precio;
                precio.Activo = dto.Activo;

                _context.SaveChanges();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Precio actualizado exitosamente",
                    Resultado = precio.IdListaPrecios
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public decimal ObtenerPrecioReferencia(string modelo, int tipoVehiculoId)
        {
            return _context.ListaPrecios
                .Where(lp => lp.Modelo == modelo &&
                       lp.TipoVehiculoId == tipoVehiculoId &&
                       lp.Activo == true)
                .Select(lp => lp.Precio)
                .FirstOrDefault();
        }
    }
}
