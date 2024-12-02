using System.ComponentModel.DataAnnotations;

namespace AutoSinco.Shared.InDTO.VehiculoDto
{
    public class VehiculoActualizacionDto
    {
        [StringLength(50, ErrorMessage = "El color no puede exceder los 50 caracteres")]
        public string? Color { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El kilometraje debe ser mayor o igual a 0")]
        public decimal? Kilometraje { get; set; }

        [Range(0, 250000000, ErrorMessage = "El valor no puede ser mayor a $250.000.000")]
        public decimal? Valor { get; set; }

        public string? ImagenUrl { get; set; }

        [StringLength(20, ErrorMessage = "El estado no puede exceder los 20 caracteres")]
        public string? Estado { get; set; }
    }
}
