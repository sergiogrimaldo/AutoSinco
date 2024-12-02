namespace AutoSinco.Shared.InDTO.ReporteDtos
{
    public class ResumenVentasPorTipoDto
    {
        public string TipoVehiculo { get; set; } = null!;
        public int CantidadVendida { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
