namespace Smarthack2021.Core.CryptoAbstractions
{
    public interface IRSAEncryption
    {
        public string RsaEncryptWithPublic(string clearText, string publicKey);

        public string RsaEncryptWithPrivate(string clearText, string privateKey);

        public string RsaDecryptWithPrivate(string base64Input, string privateKey);

        public string RsaDecryptWithPublic(string base64Input, string publicKey);
    }
}