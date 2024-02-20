using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EncreptionMethods.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<string> _availableAlgorithms;
        private int _selectedAlgorithmIndex;
        private IAlgorithmViewModel _currentAlgorithmViewModel;
        private string _inputText;
        private string _key;
        private string _result;
        private bool _isProcessing;

        public MainViewModel()
        {
            _availableAlgorithms = new ObservableCollection<string>() { "Hill Cipher", "Caesar Cipher", "RSA" };
            UpdateLabelContent(0);
            SelectedAlgorithmIndex = 0;
            CurrentAlgorithmViewModel = GetAlgorithmViewModel(SelectedAlgorithmIndex);
            InputText = "";
            Key = "";
            Result = "";
            IsProcessing = false;

            EncryptCommand = new RelayCommand(ExecuteEncrypt, CanExecuteEncrypt);
            DecryptCommand = new RelayCommand(ExecuteDecrypt, CanExecuteDecrypt);
            CopyResultCommand = new RelayCommand(ExecuteCopyResult);
        }

        private string _labelContent;
        public string LabelContent
        {
            get => _labelContent;
            set => SetProperty(ref _labelContent, value);
        }

        public void UpdateLabelContent(int algorithm)
        {
            switch (algorithm)
            {
                case 2:
                    LabelContent = "Key: as p,q,e (e.g., p=11,q=3,e=7)";
                    break;
                case 0:
                    LabelContent = "Key: n*n";
                    break;
                case 1:
                    LabelContent = "Key: shift key";
                    break;
                default:
                    LabelContent = "Key:";
                    break;
            }
        }
        public ObservableCollection<string> AvailableAlgorithms
        {
            get { return _availableAlgorithms; }
        }
        private void SelectedAlgorithmIndex_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CurrentAlgorithmViewModel = GetAlgorithmViewModel(SelectedAlgorithmIndex);
        }

        public int SelectedAlgorithmIndex
        {
            get { return _selectedAlgorithmIndex; }
            set
            {
                if (_selectedAlgorithmIndex != value)
                {
                    _selectedAlgorithmIndex = value;
                    CurrentAlgorithmViewModel = GetAlgorithmViewModel(SelectedAlgorithmIndex);
                    UpdateLabelContent(_selectedAlgorithmIndex);
                    OnPropertyChanged();
                }
            }
        }

        public IAlgorithmViewModel CurrentAlgorithmViewModel
        {
            get { return _currentAlgorithmViewModel; }
            set
            {
                if (_currentAlgorithmViewModel != value)
                {
                    _currentAlgorithmViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InputText
        {
            get { return _inputText; }
            set
            {
                SetProperty(ref _inputText, value);
                UpdateCanExecuteCommands();
            }
        }

        public string Key
        {
            get { return _key; }
            set
            {
                SetProperty(ref _key, value);
                UpdateCanExecuteCommands();
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                SetProperty(ref _result, value);
            }
        }

        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                SetProperty(ref _isProcessing, value);
            }
        }

        public ICommand EncryptCommand { get; private set; }
        public ICommand DecryptCommand { get; private set; }
        public ICommand CopyResultCommand { get; private set; }

        private IAlgorithmViewModel GetAlgorithmViewModel(int index)
        {
            switch (index)
            {
                case 0:
                    return new HillCipherViewModel();
                case 1:
                    return new CaesarCipherViewModel();
                case 2:
                    return new RsaCipherViewModel();
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }


        private void UpdateCanExecuteCommands()
        {
            (EncryptCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DecryptCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void ExecuteEncrypt()
        {
            try
            {
                IsProcessing = true;
                Result = CurrentAlgorithmViewModel.Encrypt(InputText, Key);
                IsProcessing = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            
        }

        private void ExecuteDecrypt()
        {
            try
            {
                IsProcessing = true;
                Result = CurrentAlgorithmViewModel.Decrypt(InputText, Key);
                IsProcessing = false;
            }
            catch(Exception ex )
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            
        }


        private bool CanExecuteEncrypt()
        {
            return !string.IsNullOrEmpty(InputText) && !string.IsNullOrEmpty(Key);
        }

        private bool CanExecuteDecrypt()
        {
            return !string.IsNullOrEmpty(InputText) && !string.IsNullOrEmpty(Key);
        }

        private void ExecuteCopyResult()
        {
            if (!string.IsNullOrEmpty(Result))
            {
                Clipboard.SetText(Result);
            }
        }
    }
}