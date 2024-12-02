using AutoSinco.Domain.Contracts.TipoVehiculoRepository;
using AutoSinco.Domain.Contracts.VehiculoRepository;
using AutoSinco.Infraestructure;
using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO.VehiculoDto;
using Microsoft.EntityFrameworkCore;

namespace AutoSinco.Domain.Services.VehiculoRepository
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly DBContext _context;
        private readonly ITipoVehiculoRepository _tipoVehiculoRepository;

        public VehiculoRepository(
            DBContext context,
            ITipoVehiculoRepository tipoVehiculoRepository)
        {
            _context = context;
            _tipoVehiculoRepository = tipoVehiculoRepository;
        }

        public RespuestaDto ObtenerVehiculos(int? tipoVehiculoId = null, string? estado = null, bool? esNuevo = null)
        {
            try
            {
                var query = _context.Vehiculos
                    .Include(v => v.TipoVehiculo)
                    .AsQueryable();

                if (tipoVehiculoId.HasValue)
                    query = query.Where(v => v.TipoVehiculoId == tipoVehiculoId);

                if (!string.IsNullOrEmpty(estado))
                    query = query.Where(v => v.Estado == estado);

                if (esNuevo.HasValue)
                    query = query.Where(v => v.EsNuevo == esNuevo);

                var vehiculos = query.Select(v => new
                {
                    v.IdVehiculo,
                    v.TipoVehiculo.Nombre,
                    v.Modelo,
                    v.Color,
                    v.Kilometraje,
                    v.Valor,
                    v.ImagenUrl,
                    v.FechaRegistro,
                    v.EsNuevo,
                    v.Cilindraje,
                    v.NumeroVelocidades,
                    v.Estado
                }).ToList();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Vehículos obtenidos exitosamente",
                    Resultado = vehiculos
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ObtenerVehiculoPorId(int idVehiculo)
        {
            try
            {
                var vehiculo = _context.Vehiculos
                    .Include(v => v.TipoVehiculo)
                    .Where(v => v.IdVehiculo == idVehiculo)
                    .Select(v => new
                    {
                        v.IdVehiculo,
                        v.TipoVehiculo.Nombre,
                        v.Modelo,
                        v.Color,
                        v.Kilometraje,
                        v.Valor,
                        v.ImagenUrl,
                        v.FechaRegistro,
                        v.EsNuevo,
                        v.Cilindraje,
                        v.NumeroVelocidades,
                        v.Estado
                    })
                    .FirstOrDefault();

                if (vehiculo == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Vehículo no encontrado",
                        "El vehículo especificado no existe");
                }

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Vehículo obtenido exitosamente",
                    Resultado = vehiculo
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto RegistrarVehiculo(VehiculoRegistroDto dto)
        {
            try
            {
                if (!_tipoVehiculoRepository.ExisteTipoVehiculo(dto.TipoVehiculoId))
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Tipo de vehículo inválido",
                        "El tipo de vehículo especificado no existe");
                }

                if (dto.TipoVehiculoId == 2 && dto.Cilindraje > 400) // 2 = Moto según DB
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Cilindraje inválido",
                        "El cilindraje no puede ser mayor a 400cc");
                }

                if (!ValidarLimiteInventario(dto.TipoVehiculoId))
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Límite de inventario alcanzado",
                        "No se pueden agregar más vehículos de este tipo");
                }

                if (!dto.EsNuevo && !ValidarPrecioUsado(dto.Valor, dto.Modelo, dto.TipoVehiculoId))
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Precio inválido",
                        "El valor del vehículo usado no puede ser menor al 85% del valor en la lista de precios");
                }

                var vehiculo = new Vehiculo
                {
                    TipoVehiculoId = dto.TipoVehiculoId,
                    Modelo = dto.Modelo,
                    Color = dto.Color,
                    Kilometraje = dto.Kilometraje,
                    Valor = dto.Valor,
                    ImagenUrl = dto.ImagenUrl,
                    FechaRegistro = DateTime.Now,
                    EsNuevo = dto.EsNuevo,
                    Cilindraje = dto.Cilindraje,
                    NumeroVelocidades = dto.NumeroVelocidades,
                    Estado = "Disponible"
                };

                _context.Vehiculos.Add(vehiculo);
                _context.SaveChanges();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Vehículo registrado exitosamente",
                    Resultado = vehiculo.IdVehiculo
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ActualizarVehiculo(int idVehiculo, VehiculoActualizacionDto dto)
        {
            try
            {
                var vehiculo = _context.Vehiculos.Find(idVehiculo);
                if (vehiculo == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Vehículo no encontrado",
                        "El vehículo especificado no existe");
                }

                if (dto.Color != null)
                    vehiculo.Color = dto.Color;
                if (dto.Kilometraje.HasValue)
                    vehiculo.Kilometraje = dto.Kilometraje.Value;
                if (dto.Valor.HasValue)
                    vehiculo.Valor = dto.Valor.Value;
                if (dto.ImagenUrl != null)
                    vehiculo.ImagenUrl = dto.ImagenUrl;
                if (dto.Estado != null)
                    vehiculo.Estado = dto.Estado;

                _context.SaveChanges();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Vehículo actualizado exitosamente",
                    Resultado = vehiculo.IdVehiculo
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto CambiarEstadoVehiculo(int idVehiculo, string estado)
        {
            try
            {
                var vehiculo = _context.Vehiculos.Find(idVehiculo);
                if (vehiculo == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Vehículo no encontrado",
                        "El vehículo especificado no existe");
                }

                vehiculo.Estado = estado;
                _context.SaveChanges();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Estado del vehículo actualizado exitosamente",
                    Resultado = vehiculo.IdVehiculo
                };
            }
            catch (Exception)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public bool ValidarLimiteInventario(int tipoVehiculoId)
        {
            var limiteInventario = _tipoVehiculoRepository.ObtenerLimiteInventario(tipoVehiculoId);
            var cantidadActual = _context.Vehiculos.Count(v =>
                v.TipoVehiculoId == tipoVehiculoId &&
                v.Estado == "Disponible");

            return cantidadActual < limiteInventario;
        }

        public bool ValidarPrecioUsado(decimal precioUsado, string modelo, int tipoVehiculoId)
        {
            var precioNuevo = _context.ListaPrecios
                .Where(lp => lp.Modelo == modelo &&
                       lp.TipoVehiculoId == tipoVehiculoId &&
                       lp.Activo == true)
                .Select(lp => lp.Precio)
                .FirstOrDefault();

            if (precioNuevo == 0) return true; // Si no hay precio de referencia, permitir cualquier valor

            var precioMinimo = precioNuevo * 0.85m;
            return precioUsado >= precioMinimo;
        }
    }
}
