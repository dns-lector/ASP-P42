namespace ASP_P42.Services.Kdf
{
    //  KDF key derivation function By RFC 2898 https://datatracker.ietf.org/doc/html/rfc2898
    public interface IKdfService
    {
        String Dk(String password, String salt);
    }
}
