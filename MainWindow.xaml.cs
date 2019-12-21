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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint =  new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            socket.Connect(endPoint);

            string msg = message.Text;

            byte[] buffer = Encoding.Default.GetBytes(msg);
            socket.Send(buffer, 0, buffer.Length, 0);

            buffer = new byte[255];
            int rec = socket.Receive(buffer, 0, buffer.Length, 0);
            Array.Resize(ref buffer, rec);
            Console.WriteLine("Received: {0}", Encoding.Default.GetString(buffer));

            socket.Close();
        }

    }
}
