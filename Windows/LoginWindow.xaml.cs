using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private Socket socket;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            // login logic
            try
            {
                alertText.Text = "Logowanie";
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
                socket.Connect(endPoint);

                MainWindow window = new MainWindow(socket);
                UiControl.ChangeWindow(this, window);
            }
            catch(Exception ex)
            {
                alertText.Text = "Server nieodpowiada";
            }


        }
        private void RegistrationPage(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow();
            UiControl.ChangeWindow(this, window);
        }


    }
}
