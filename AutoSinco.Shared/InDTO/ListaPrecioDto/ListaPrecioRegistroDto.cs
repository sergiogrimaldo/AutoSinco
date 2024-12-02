using System.ComponentModel.DataAnnotations;

namespace AutoSinco.Shared.InDTO.ListaPrecioDto
{
    public class ListaPrecioRegistroDto
    {
        [Required(ErrorMessage = "El modelo es requerido")]
        [StringLength(100, ErrorMessage = "El modelo no puede exceder los 100 caracteres")]
        public string Modelo { get; set; } = null!;

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0, 250000000, ErrorMessage = "El precio no puede ser mayor a $250.000.000")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El tipo de vehículo es requerido")]
        public int TipoVehiculoId { get; set; }
    }
}
