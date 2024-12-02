namespace AutoSinco.Shared.InDTO.ReporteDtos
{
    public class ResumenVehiculosDto
    {
        public int TipoVehiculoId { get; set; }
        public string TipoVehiculo { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public int CantidadDisponible { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorPromedio { get; set; }
    }
}
