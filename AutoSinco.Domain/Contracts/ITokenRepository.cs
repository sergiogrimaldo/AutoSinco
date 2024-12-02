using AutoSinco.Infraestructure;
using AutoSinco.Shared.GeneralDTO;

namespace AutoSinco.Domain.Contracts
{
    public interface ITokenRepository
    {
        string GenerarToken(Usuario usuario, string ip);
        bool CancelarToken(string token);
        Object ObtenerInformacionToken(string token);
        ValidoDTO EsValido(string idToken, string idUsuario, string ip);
        void AumentarTiempoExpiracion(string token);
    }
}