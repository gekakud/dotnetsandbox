using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DigitalSignatureProvider
{
    public class DigitalSignature
    {
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;

        public bool SignDocument(string p_docPath, string p_algType)
        {
            var hashedDocument = DocumentParseAndHash(p_docPath);

            AssignNewKey();

            StoreHashedStuff(hashedDocument, p_docPath);

            var signature = SignHashedDocument(hashedDocument);

            return VerifySignedDocument(hashedDocument,signature);
        }

        private void StoreHashedStuff(byte[] p_hashedDocument, string p_path)
        {
            var new_path = p_path.Insert(0, "new_");
            File.WriteAllBytes(new_path, p_hashedDocument);
        }

        public bool VerifySignedDocument(byte[] p_hashedDocument, byte[] p_signature)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportParameters(_publicKey);

                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(p_hashedDocument, p_signature);
            }
        }   

        private void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);
            }
        }

        private byte[] SignHashedDocument(byte[] p_hashOfDataToSign)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_privateKey);

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                return rsaFormatter.CreateSignature(p_hashOfDataToSign);
            }
        }

        private byte[] DocumentParseAndHash(string p_docPath)
        {
            //var fileData = File.ReadAllBytes(p_docPath);
            var fileData = Encoding.UTF8.GetBytes("Hello me!");
            byte[] hashedDocument;

            using (var sha256 = SHA256.Create())
            {
                hashedDocument = sha256.ComputeHash(fileData);
            }

            return hashedDocument;
        }
    }
}
