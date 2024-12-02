using AutoSinco.Domain.Contracts;
using AutoSinco.Shared.InDTO;
using Microsoft.Extensions.Options;

namespace AutoSinco.Domain.Services
{
    public class ConstantesService : IConstantesService
    {
        private readonly UsuarioSettings _settings;

        public ConstantesService(IOptions<UsuarioSettings> settings)
        {
            _settings = settings.Value;
        }

        public string ObtenerPrefijoUsuario(int rolId)
        {
            return rolId switch
            {
                var id when id == _settings.Roles.Admin => _settings.Prefijos.Admin,
                var id when id == _settings.Roles.Vendedor => _settings.Prefijos.Vendedor,
                var id when id == _settings.Roles.Gerente => _settings.Prefijos.Gerente,
                var id when id == _settings.Roles.Inventario => _settings.Prefijos.Inventario,
                _ => throw new ArgumentException($"RolId no válido: {rolId}")
            };
        }

        public bool EsRolValido(int rolId)
        {
            return ObtenerRolesValidos().Contains(rolId);
        }

        public int[] ObtenerRolesValidos()
        {
            return new[]
            {
            _settings.Roles.Admin,
            _settings.Roles.Vendedor,
            _settings.Roles.Gerente,
            _settings.Roles.Inventario
        };
        }
    }
}
