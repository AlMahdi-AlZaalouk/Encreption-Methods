using System.Text;
using System;

public class CaesarCipherAlgorithm
{
    public string Encrypt(string plaintext, int key)
    {
        if (string.IsNullOrEmpty(plaintext))
        {
            throw new ArgumentException("Plaintext cannot be empty.");
        }

        StringBuilder encryptedText = new StringBuilder(plaintext);
        for (int i = 0; i < encryptedText.Length; i++)
        {
            if (char.IsLetter(encryptedText[i]))
            {
                char shiftedChar = (char)(encryptedText[i] + key);
                if (char.IsLower(encryptedText[i]) && shiftedChar > 'z')
                {
                    shiftedChar = (char)((shiftedChar % 'z') + 'a' - 1);
                }
                else if (char.IsUpper(encryptedText[i]) && shiftedChar > 'Z')
                {
                    shiftedChar = (char)((shiftedChar % 'Z') + 'A' - 1);
                }
                encryptedText[i] = shiftedChar;
            }
        }

        return encryptedText.ToString();
    }

    public string Decrypt(string ciphertext, int key)
    {
        if (string.IsNullOrEmpty(ciphertext))
        {
            throw new ArgumentException("Ciphertext cannot be empty.");
        }

        StringBuilder decryptedText = new StringBuilder(ciphertext);
        for (int i = 0; i < decryptedText.Length; i++)
        {
            if (char.IsLetter(decryptedText[i]))
            {
                char shiftedChar = (char)(decryptedText[i] - key);
                if (char.IsLower(decryptedText[i]) && shiftedChar < 'a')
                {
                    shiftedChar = (char)(('z' + 1) - ('a' - shiftedChar));
                }
                else if (char.IsUpper(decryptedText[i]) && shiftedChar < 'A')
                {
                    shiftedChar = (char)(('Z' + 1) - ('A' - shiftedChar));
                }
                decryptedText[i] = shiftedChar;
            }
        }

        return decryptedText.ToString();
    }
}
