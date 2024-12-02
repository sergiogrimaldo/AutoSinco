using System;
using System.Collections.Generic;

namespace AutoSinco.Infraestructure;

public partial class TipoVehiculo
{
    public int IdTipoVehiculo { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int LimiteInventario { get; set; }

    public virtual ICollection<ListaPrecio> ListaPrecios { get; set; } = new List<ListaPrecio>();

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
