using System.ComponentModel.DataAnnotations;

namespace AutoSinco.Shared.InDTO.VehiculoDto
{
    public class VehiculoRegistroDto
    {
        [Required(ErrorMessage = "El tipo de vehículo es requerido")]
        public int TipoVehiculoId { get; set; }

        [Required(ErrorMessage = "El modelo es requerido")]
        [StringLength(100, ErrorMessage = "El modelo no puede exceder los 100 caracteres")]
        public string Modelo { get; set; } = null!;

        [Required(ErrorMessage = "El color es requerido")]
        [StringLength(50, ErrorMessage = "El color no puede exceder los 50 caracteres")]
        public string Color { get; set; } = null!;

        [Required(ErrorMessage = "El kilometraje es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El kilometraje debe ser mayor o igual a 0")]
        public decimal Kilometraje { get; set; }

        [Required(ErrorMessage = "El valor es requerido")]
        [Range(0, 250000000, ErrorMessage = "El valor no puede ser mayor a $250.000.000")]
        public decimal Valor { get; set; }

        public string? ImagenUrl { get; set; }

        [Required(ErrorMessage = "Debe especificar si el vehículo es nuevo")]
        public bool EsNuevo { get; set; }

        [Range(0, 400, ErrorMessage = "El cilindraje no puede ser mayor a 400cc")]
        public int? Cilindraje { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El número de velocidades debe ser mayor a 0")]
        public int? NumeroVelocidades { get; set; }
    }
}
