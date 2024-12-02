using System;
using System.Collections.Generic;

namespace AutoSinco.Infraestructure;

public partial class Usuario
{
    public string IdUsuario { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int RolId { get; set; }

    public bool? Activo { get; set; }

    public virtual Rol Rol { get; set; } = null!;

    public virtual ICollection<TokenExpirado> TokenExpirados { get; set; } = new List<TokenExpirado>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
