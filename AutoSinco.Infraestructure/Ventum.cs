using System;
using System.Collections.Generic;

namespace AutoSinco.Infraestructure;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public int IdVehiculo { get; set; }

    public string NombreComprador { get; set; } = null!;

    public string DocumentoComprador { get; set; } = null!;

    public DateTime? FechaVenta { get; set; }

    public decimal ValorVenta { get; set; }

    public string IdUsuarioVendedor { get; set; } = null!;

    public virtual Usuario IdUsuarioVendedorNavigation { get; set; } = null!;

    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;
}
