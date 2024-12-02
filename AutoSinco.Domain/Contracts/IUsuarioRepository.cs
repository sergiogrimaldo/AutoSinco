using AutoSinco.Shared.GeneralDTO;
using AutoSinco.Shared.InDTO;

namespace AutoSinco.Domain.Contracts
{
    public interface IUsuarioRepository
    {
        RespuestaDto AutenticarUsuario(UsuarioLoginDto args);
        RespuestaDto RegistrarUsuario(UsuarioRegistroDto args);
    }
}