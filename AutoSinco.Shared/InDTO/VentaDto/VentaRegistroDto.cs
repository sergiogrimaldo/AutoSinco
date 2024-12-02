using System.ComponentModel.DataAnnotations;

namespace AutoSinco.Shared.InDTO.VentaDto
{
    public class VentaRegistroDto
    {
        [Required(ErrorMessage = "El ID del vehículo es requerido")]
        public int IdVehiculo { get; set; }

        [Required(ErrorMessage = "El nombre del comprador es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string NombreComprador { get; set; } = null!;

        [Required(ErrorMessage = "El documento del comprador es requerido")]
        [StringLength(20, ErrorMessage = "El documento no puede exceder los 20 caracteres")]
        public string DocumentoComprador { get; set; } = null!;

        [Required(ErrorMessage = "El valor de la venta es requerido")]
        [Range(0, 250000000, ErrorMessage = "El valor no puede ser mayor a $250.000.000")]
        public decimal ValorVenta { get; set; }

        [Required(ErrorMessage = "El ID del vendedor es requerido")]
        public string IdUsuarioVendedor { get; set; } = null!;
    }

}
