using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EncreptionMethods.HillCipher.ViewModel;
using EncreptionMethods.HillCipher; // Ensure this using directive matches your model's namespace
using System.Windows;

namespace EncreptionMethods.HillCipher.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _inputText;
        private string _key;
        private string _result;

        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetProperty(ref _isProcessing, value);
        }

        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public ICommand EncryptCommand { get; }
        public ICommand DecryptCommand { get; }

        public MainViewModel()
        {
            IsProcessing = false;
            EncryptCommand = new RelayCommand(async () => await ProcessText(true));
            DecryptCommand = new RelayCommand(async () => await ProcessText(false));
        }
        private async Task ProcessText(bool isEncrypting)
        {
            IsProcessing = true;

            try
            {
                await Task.Delay(1000); // Replace with actual encryption/decryption call

                HillCipherAlgorithm cipher = new HillCipherAlgorithm(Key);
                Result = isEncrypting ? cipher.Encrypt(InputText) : cipher.Decrypt(InputText);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
}
