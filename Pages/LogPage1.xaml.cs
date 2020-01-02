using Client.Windows;
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

namespace Client.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy LogPage1.xaml
    /// </summary>
    public partial class LogPage1 : Page
    {
        private static Window parentWin;
        public LogPage2 page2;
        private Socket socket;

        private Serializer serializer = new Serializer();
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];



        public LogPage1(Window parent, Socket socketParam)
        {
            this.socket = socketParam;
            page2 = new LogPage2(parent, socket);
            parentWin = parent;
            InitializeComponent();
        }

        public void serverError()
        {
            LoginWindow loginWindow = new LoginWindow();
            UiControl.ChangeWindow(parentWin, loginWindow);
            return;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            // login validation
            // to be done

            int accNumber;
            string stringAccNumber = numberInput.Text;
            string password = passwordInput.Password;
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
                Order loginOrder = new Order(3, accNumber, password);
                byte[] sendbuff = serializer.Serialize_Obj(loginOrder);
                try
                {
                    socket.Send(sendbuff, sendbuff.Length, 0);
                }
                catch (SocketException) { serverError(); }
                // socket.receive(user profile serialized) and then deserialize
                byte[] recBuff = new byte[2048]; 
                socket.Receive(recBuff, 0, recBuff.Length, 0);

                Order loginVerify = (Order)serializer.Deserialize_Obj(recBuff, new Order());
                User profile = (User)serializer.Deserialize_Obj(loginVerify.acc, new User());

                Console.WriteLine(profile);

                MainWindow window = new MainWindow(socket, profile);
                UiControl.ChangeWindow(parentWin, window);
            }
            catch (SocketException ex)
            {
                alertText.Text = ex.Message;
                loginButton.IsEnabled = true;
            }
        }

        private void RegistrationPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(page2);
        }
    }
}
