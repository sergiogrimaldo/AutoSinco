using System.ComponentModel.DataAnnotations;

namespace AutoSinco.Shared.InDTO
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Contraseña { get; set; }

        public string? Ip { get; set; }
    }
}
