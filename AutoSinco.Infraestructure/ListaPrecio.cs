using System;
using System.Collections.Generic;

namespace AutoSinco.Infraestructure;

public partial class ListaPrecio
{
    public int IdListaPrecios { get; set; }

    public string Modelo { get; set; } = null!;

    public decimal Precio { get; set; }

    public int TipoVehiculoId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? Activo { get; set; }

    public virtual TipoVehiculo TipoVehiculo { get; set; } = null!;
}
