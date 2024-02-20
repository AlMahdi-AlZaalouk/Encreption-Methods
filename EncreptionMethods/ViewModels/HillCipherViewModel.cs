using System;
using EncreptionMethods;
using EncreptionMethods.ViewModels;

public class HillCipherViewModel : ViewModelBase, IAlgorithmViewModel
{
    private string _key;
    private HillCipherAlgorithm _cipher;

    public HillCipherViewModel()
    {
        _cipher = new HillCipherAlgorithm("");
    }

    public string Key
    {
        get { return _key; }
        set
        {
            _key = value;
            _cipher = new HillCipherAlgorithm(_key);
            OnPropertyChanged();
        }
    }

    public string Encrypt(string plaintext, string key)
    {
        Key = key;
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key cannot be empty.");
        }

        return _cipher.Encrypt(plaintext);
    }

    public string Decrypt(string ciphertext, string key)
    {
        Key = key;
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key cannot be empty.");
        }

        return _cipher.Decrypt(ciphertext);
    }
}
