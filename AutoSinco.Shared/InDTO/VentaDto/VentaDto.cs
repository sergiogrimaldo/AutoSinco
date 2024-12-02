namespace AutoSinco.Shared.InDTO.VentaDto
{
    public class VentaDto
    {
        public int IdVenta { get; set; }
        public int IdVehiculo { get; set; }
        public string NombreComprador { get; set; } = null!;
        public string DocumentoComprador { get; set; } = null!;
        public DateTime? FechaVenta { get; set; }
        public decimal ValorVenta { get; set; }
        public string IdUsuarioVendedor { get; set; } = null!;
        public string NombreVendedor { get; set; } = null!;
        public string ModeloVehiculo { get; set; } = null!;
        public string TipoVehiculo { get; set; } = null!;
    }
}
