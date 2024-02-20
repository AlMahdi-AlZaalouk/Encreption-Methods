using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using EncreptionMethods.ViewModels; // Assuming this namespace exists

public class CaesarCipherViewModel : ViewModelBase, IAlgorithmViewModel
{
    private string _plaintext;
    private int _key;
    private string _result;
    private CaesarCipherAlgorithm _cipher;

    public CaesarCipherViewModel()
    {
        _plaintext = "";
        _key = 0;
        _result = "";
        _cipher = new CaesarCipherAlgorithm();
    }

    public string Plaintext
    {
        get { return _plaintext; }
        set
        {
            _plaintext = value;
            OnPropertyChanged();
        }
    }

    public int Key
    {
        get { return _key; }
        set
        {
            _key = value;
            OnPropertyChanged();
        }
    }

    public string Result
    {
        get { return _result; }
        set
        {
            _result = value;
            OnPropertyChanged();
        }
    }

    public bool CheckValidInput(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            MessageBox.Show("Error: Please enter a message.", "ERROR");
            return false;
        }

        foreach (char c in str)
        {
            if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
            {
                MessageBox.Show("Error: Please enter a valid message containing only letters and spaces.", "ERROR");
                return false;
            }
        }

        return true;
    }

    public string Encrypt(string plaintext, string key)
    {
        if (!CheckValidInput(plaintext))
        {
            return "";
        }

        try
        {
            int keyValue = Convert.ToInt32(key);
            Result = _cipher.Encrypt(plaintext, keyValue);
            return Result;
        }
        catch (FormatException ex)
        {
            MessageBox.Show("Error: Please enter a valid integer key.", "Error");
            return "";
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Error");
            return "";
        }
    }

    public string Decrypt(string ciphertext, string key)
    {
        if (!CheckValidInput(ciphertext))
        {
            return "";
        }

        try
        {
            int keyValue = Convert.ToInt32(key);
            Result = _cipher.Decrypt(ciphertext, keyValue);
            return Result;
        }
        catch (FormatException ex)
        {
            MessageBox.Show("Error: Please enter a valid integer key.", "Error");
            return "";
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Error");
            return "";
        }
    }
}
