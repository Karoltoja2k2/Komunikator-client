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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static Socket socket;
        private byte[] buffer;


        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            // Authentication logic
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            socket.Connect(endPoint);

            MainWindow win = new MainWindow(socket);
            App.Current.MainWindow = win;
            this.Close();
            win.Show();

        }

    }
}
