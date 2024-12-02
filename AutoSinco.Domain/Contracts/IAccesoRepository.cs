namespace AutoSinco.Domain.Contracts
{
    public interface IAccesoRepository
    {
        bool ValidarAcceso(string sitio, string contraseña);
    }  
}