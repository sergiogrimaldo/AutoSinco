namespace AutoSinco.Domain.Services
{
    public partial class UsuarioRepository
    {
        private string GenerarIdUsuario(int rolId)
        {
            string prefix = ObtenerPrefijoPorRol(rolId);
            var usuarios = _context.Usuarios
                .Where(u => u.IdUsuario.StartsWith(prefix))
                .ToList();

            if (!usuarios.Any())
            {
                return $"{prefix}000001";
            }

            var maxId = usuarios
                .Select(u =>
                {
                    if (int.TryParse(u.IdUsuario.Substring(prefix.Length), out int id))
                        return id;
                    return 0;
                })
                .Max();

            return $"{prefix}{(maxId + 1):D6}";
        }

        private string ObtenerPrefijoPorRol(int rolId)
        {
            if (rolId == _usuarioSettings.Roles.Admin)
                return _usuarioSettings.Prefijos.Admin;
            if (rolId == _usuarioSettings.Roles.Vendedor)
                return _usuarioSettings.Prefijos.Vendedor;
            if (rolId == _usuarioSettings.Roles.Gerente)
                return _usuarioSettings.Prefijos.Gerente;
            if (rolId == _usuarioSettings.Roles.Inventario)
                return _usuarioSettings.Prefijos.Inventario;

            throw new ArgumentException($"RolId no válido: {rolId}");
        }

        private bool EsRolValido(int rolId)
        {
            return rolId == _usuarioSettings.Roles.Admin ||
                   rolId == _usuarioSettings.Roles.Vendedor ||
                   rolId == _usuarioSettings.Roles.Gerente ||
                   rolId == _usuarioSettings.Roles.Inventario;
        }

        private string ObtenerNombreRol(int rolId)
        {
            return rolId switch
            {
                var id when id == _usuarioSettings.Roles.Admin => "Administrador",
                var id when id == _usuarioSettings.Roles.Gerente => "Gerente",
                var id when id == _usuarioSettings.Roles.Vendedor => "Vendedor",
                var id when id == _usuarioSettings.Roles.Inventario => "Inventario",
                _ => "Rol Desconocido"
            };
        }
    }
}
