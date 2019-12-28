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
using System.Windows.Shapes;
using Client.Windows;

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            number.Text = RandomNumber();
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            UiControl.ChangeWindow(this, window);
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            string nr = number.Text;
            string eMail = email.Text;
            string p1 = password1.Password;
            string p2 = password2.Password;
            bool agr = (bool)agreement.IsChecked;

            if (!eMail.Contains("@"))
            {
                valid = false;
                email.BorderBrush = Brushes.Red;
            }
            else
            {
                email.BorderBrush = Brushes.Green;
            }

            if (p1 != p2)
            {
                valid = false;
                password1.BorderBrush = Brushes.Red;
                password2.BorderBrush = Brushes.Red;
            }
            else
            {
                password1.BorderBrush = Brushes.Green;
                password2.BorderBrush = Brushes.Green;
            }

            if (passwordValidation(p1) == false | passwordValidation(p2) == false)
            {
                valid = false;
                password1.BorderBrush = Brushes.Red;
                password2.BorderBrush = Brushes.Red;
            }
            else
            {
                password1.BorderBrush = Brushes.Green;
                password2.BorderBrush = Brushes.Green;
            }

            if (agr != true)
            {
                valid = false;
                agreement.BorderBrush = Brushes.Red;
            }
            else
            {
                agreement.BorderBrush = Brushes.Green;
            }

            if (valid)
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"C:\Users\Karol\Desktop\C#\Komunikator\Client\accounts.txt", true))
                {
                    file.WriteLine($"{nr};{eMail};{p1}");
                    
                    LoginWindow window = new LoginWindow();
                    UiControl.ChangeWindow(this, window);
                }
            }
            else
            {
                alertText.Text = "Popraw lub uzupełnij czerwone pola, skorzystaj z podpowiedzi wskazując myszką na pole";
            }


        }

        private bool passwordValidation(string password)
        {
            string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            if (password.Length <= 8)
                return false;

            foreach (char character in password)
            {
                if (!validChars.Contains(character))
                    return false;
            }

            return true;

        }

        private void NewNumber(object sender, RoutedEventArgs e)
        {
            number.Text = RandomNumber();
        }

        private string RandomNumber()
        {
            var chars = "0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }
    }
}
