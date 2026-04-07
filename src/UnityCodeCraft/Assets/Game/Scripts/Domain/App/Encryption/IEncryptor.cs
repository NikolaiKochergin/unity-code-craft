namespace App.Encryption
{
    public interface IEncryptor
    {
        byte[] Encrypt(byte[] input);
        byte[] Decrypt(byte[] input);
    }
}