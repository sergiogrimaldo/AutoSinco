namespace AutoSinco.Shared.InDTO.ListaPrecioDto
{
    public class ListaPrecioDto
    {
        public int IdListaPrecios { get; set; }
        public string Modelo { get; set; } = null!;
        public decimal Precio { get; set; }
        public int TipoVehiculoId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Activo { get; set; }
        public string? TipoVehiculoNombre { get; set; }
    }
}
