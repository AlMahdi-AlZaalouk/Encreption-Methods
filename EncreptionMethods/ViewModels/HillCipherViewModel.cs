using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EncreptionMethods.ViewModels
{
    public class HillCipherViewModel : ViewModelBase
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
        public ICommand CopyResultCommand { get; }

        public HillCipherViewModel()
        {
            IsProcessing = false;
            EncryptCommand = new HillCipherRelayCommand(async () => await ProcessText(true));
            DecryptCommand = new HillCipherRelayCommand(async () => await ProcessText(false));
            CopyResultCommand = new HillCipherRelayCommand(CopyResultToClipboard);
        }
        private void CopyResultToClipboard()
        {
            if (!string.IsNullOrWhiteSpace(Result))
            {
                Clipboard.SetText(Result);
                //MessageBox.Show("Result copied to clipboard!");
            }
        }
        private async Task ProcessText(bool isEncrypting)
        {
            IsProcessing = true;

            try
            {
                string inputText = InputText.ToUpper();
                string key = Key.ToUpper();
                await Task.Delay(1000);
                HillCipherAlgorithm cipher = new HillCipherAlgorithm(key);
                Result =  isEncrypting ? cipher.Encrypt(inputText) : cipher.Decrypt(inputText);
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
