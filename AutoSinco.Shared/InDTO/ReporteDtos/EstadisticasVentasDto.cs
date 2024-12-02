namespace AutoSinco.Shared.InDTO.ReporteDtos
{
    public class EstadisticasVentasDto
    {
        public int TotalVentas { get; set; }
        public decimal ValorTotalVentas { get; set; }
        public decimal PromedioVentaDiaria { get; set; }
        public List<ResumenVentasPorTipoDto> VentasPorTipo { get; set; } = new();
    }
}
