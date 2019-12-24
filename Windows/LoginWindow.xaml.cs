using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private Socket socket;
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            int accNumber;
            // login logic
            string stringAccNumber = numberInput.Text;
            if (String.IsNullOrEmpty(stringAccNumber))
            {
                accNumber = 21372137;
            }
            else
            {
                accNumber = Int32.Parse(stringAccNumber);
            }
            

            try
            {
                alertText.Text = "Logowanie";
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);

                // connect and receive session token
                socket.Connect(endPoint);
                int bytesRec = socket.Receive(buffer);
                string token = $"{Encoding.ASCII.GetString(buffer, 0, bytesRec)}";

                User user = new User(accNumber, token);
                byte[] userData = user.ToByteArray();
                // send account info and token to server
                socket.Send(userData, 0, userData.Length, SocketFlags.None);
                //           
                MainWindow window = new MainWindow(socket, user);
                UiControl.ChangeWindow(this, window);
            }
            catch(SocketException ex)
            {
                alertText.Text = ex.Message;
                loginButton.IsEnabled = true;
            }
        }


        private void RegistrationPage(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow();
            UiControl.ChangeWindow(this, window);
        }


    }
}
