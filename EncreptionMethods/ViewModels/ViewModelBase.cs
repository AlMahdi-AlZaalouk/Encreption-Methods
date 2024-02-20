using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace EncreptionMethods.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            try
            {
                storage = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            catch (Exception ex)
            {
                // Handle potential exceptions during property setting
                // Log the error or display a user-friendly message
                Console.WriteLine($"Error setting property '{propertyName}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Copies the specified text to the clipboard and optionally shows a user feedback message.
        /// </summary>
        /// <param name="text">The text to copy.</param>
        /// <param name="successMessage">The message to display on successful copying (optional).</param>
        protected void CopyToClipboard(string text, string successMessage = null)
        {
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    Clipboard.SetText(text);
                    if (!string.IsNullOrEmpty(successMessage))
                    {
                        // Show user feedback (e.g., message box or status bar notification)
                        Console.WriteLine(successMessage); // Replace with your preferred feedback mechanism
                    }
                }
                catch (Exception ex)
                {
                    // Handle potential exceptions during clipboard operations
                    // Log the error or display a user-friendly message
                    Console.WriteLine($"Error copying text to clipboard: {ex.Message}");
                }
            }
        }
    }
}
