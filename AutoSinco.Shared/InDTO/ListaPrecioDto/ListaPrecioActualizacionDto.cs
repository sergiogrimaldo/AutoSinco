using System.ComponentModel.DataAnnotations;

namespace AutoSinco.Shared.InDTO.ListaPrecioDto
{
    public class ListaPrecioActualizacionDto
    {
        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0, 250000000, ErrorMessage = "El precio no puede ser mayor a $250.000.000")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Activo { get; set; }
    }
}
