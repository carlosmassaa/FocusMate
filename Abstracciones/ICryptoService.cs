namespace Abstracciones
{
    public interface ICryptoService
    {
        byte[] GenerarSalt();
        byte[] CalcularHash(string password, byte[] salt);
        bool VerificarPassword(string password, byte[] salt, byte[] hash);
        
        long CalcularDVH(string cadena);
        long CalcularDVV(string[] dvhStringsOrdenados);
    }
}