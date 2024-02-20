using System;
using EncreptionMethods.ViewModels;

namespace EncreptionMethods.ViewModels
{
    public class RsaCipherViewModel : ViewModelBase, IAlgorithmViewModel
    {
        private RsaAlgorithm _rsaAlgorithm;
        private string _message;
        private string _ciphertext;
        private string _key; // This will contain p, q, and e values in a comma-separated format

        public RsaCipherViewModel()
        {
            _rsaAlgorithm = new RsaAlgorithm();
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string Ciphertext
        {
            get => _ciphertext;
            set => SetProperty(ref _ciphertext, value);
        }

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public string Encrypt(string plaintext, string key)
        {
            ParseKey(key);
            try
            {
                int message = Convert.ToInt32(plaintext); // Assuming message is numeric for simplicity
                int encryptedMessage = _rsaAlgorithm.Encrypt(message);
                Ciphertext = encryptedMessage.ToString();
                return Ciphertext;
            }
            catch (Exception ex)
            {
                // Log or handle exception
                return $"Encryption failed: {ex.Message}";
            }
        }

        public string Decrypt(string ciphertext, string key)
        {
            ParseKey(key);
            try
            {
                int encryptedMessage = Convert.ToInt32(ciphertext);
                string decryptedMessage = _rsaAlgorithm.Decrypt(encryptedMessage).ToString();
                Message = decryptedMessage;
                return Message;
            }
            catch (Exception ex)
            {
                // Log or handle exception
                return $"Decryption failed: {ex.Message}";
            }
        }

        private void ParseKey(string key)
        {
            var parts = key.Split(',');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Key must be in the format 'p,q,e'");
            }

            int p = int.Parse(parts[0]);
            int q = int.Parse(parts[1]);
            int e = int.Parse(parts[2]);

            _rsaAlgorithm = new RsaAlgorithm(p, q, e); // Re-initialize the RSA algorithm with new parameters
        }
    }
}
