using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using Client.Windows;

namespace Client.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy LogPage2.xaml
    /// </summary>
    public partial class LogPage2 : Page
    {
        private static Window parentWin;
        public Socket socket;

        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        public Serializer serializer = new Serializer();

        public LogPage2(Window parent, Socket socketParam)
        {
            this.socket = socketParam;
            parentWin = parent;
            InitializeComponent();
            number.Text = RandomNumber();
        }

        public void serverError()
        {
            LoginWindow loginWindow = new LoginWindow();
            UiControl.ChangeWindow(parentWin, loginWindow);
            return;
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            int nr = Int32.Parse(number.Text);
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
                Order order = new Order(4, nr, eMail, p1);

                byte[] sendBuff = serializer.Serialize_Obj(order);
                try
                {
                    socket.Send(sendBuff, sendBuff.Length, 0);
                }
                catch (SocketException) { serverError(); }

                byte[] recBuff = new byte[2048];
                try
                {
                    socket.Receive(recBuff, recBuff.Length, 0);
                }
                catch (SocketException) { serverError(); }

                Order response = (Order)serializer.Deserialize_Obj(recBuff, new Order());
                if (response.succes == true && response.orderType == 5)
                    NavigationService.GoBack();
                else
                    alertText.Text = "Rejestracja się nie udała i w sumie nie wiem czemu xD";

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

        private void LoginPage(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
