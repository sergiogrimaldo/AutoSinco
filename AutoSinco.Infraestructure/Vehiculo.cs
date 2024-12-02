using System;
using System.Collections.Generic;

namespace AutoSinco.Infraestructure;

public partial class Vehiculo
{
    public int IdVehiculo { get; set; }

    public int TipoVehiculoId { get; set; }

    public string Modelo { get; set; } = null!;

    public string Color { get; set; } = null!;

    public decimal Kilometraje { get; set; }

    public decimal Valor { get; set; }

    public string? ImagenUrl { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool EsNuevo { get; set; }

    public int? Cilindraje { get; set; }

    public int? NumeroVelocidades { get; set; }

    public string? Estado { get; set; }

    public virtual TipoVehiculo TipoVehiculo { get; set; } = null!;

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
