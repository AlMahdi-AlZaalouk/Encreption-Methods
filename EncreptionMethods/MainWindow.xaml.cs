using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EncreptionMethods
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string PlaintextHint = "Enter plaintext or ciphertext";
        private const string KeyHint = "Enter key (length must be a perfect square)";
        public MainWindow()
        {
            InitializeComponent();
            txtPlaintext.Text = PlaintextHint;
            txtKey.Text = KeyHint;
        }


        // The HillCipher class goes here (from the previous example)

        private void btnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            ProcessText(isEncrypting: true);
        }

        private void btnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            ProcessText(isEncrypting: false);
        }

        private void ProcessText(bool isEncrypting)
        {
            string inputText = txtPlaintext.Text.ToUpper();
            string key = txtKey.Text.ToUpper();

            try
            {
                HillCipher cipher = new HillCipher(key);
                string result = isEncrypting ? cipher.Encrypt(inputText) : cipher.Decrypt(inputText);
                lblResult.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox == txtPlaintext && textBox.Text == PlaintextHint)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
            else if (textBox == txtKey && textBox.Text == KeyHint)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void Txt_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox == txtPlaintext && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = PlaintextHint;
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
            else if (textBox == txtKey && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = KeyHint;
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
        private void LblResult_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lblResult.Text))
            {
                Clipboard.SetText(lblResult.Text);
                MessageBox.Show("Result copied to clipboard!");
            }
        }

    }
}
