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

namespace Client
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private byte[] buffer;
        private Socket socket;
        private bool connected = false;
        private string connectionKey;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Send_Msg(object sender, RoutedEventArgs e)
        {
            if (socket != null)
            {
                string msg = messageInput.Text;
                if (!String.IsNullOrEmpty(msg))
                {
                    buffer = Encoding.Default.GetBytes(msg);
                    socket.Send(buffer, 0, buffer.Length, 0);
                    messageInput.Text = "";
                }
            }
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            if (connected == false)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
                socket.Connect(endPoint);
                buffer = new byte[255];
                int rec = socket.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rec);
                connectionKey = Encoding.Default.GetString(buffer);
            }
            else
            {
                buffer = Encoding.Default.GetBytes(connectionKey);
                socket.Send(buffer, 0, buffer.Length, 0);
                socket.Close();
            }

            connected = !connected;

            // byte[] buffer = new byte[255];
            // int rec = socket.Receive(buffer, 0, buffer.Length, 0);
            // Array.Resize(ref buffer, rec);
        }

    }
}
