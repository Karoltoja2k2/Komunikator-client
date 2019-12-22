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
    public partial class ChatWindow : Window
    {

        private byte[] buffer;


        public ChatWindow()
        {
            InitializeComponent();
        }

        private void Send_Msg(object sender, RoutedEventArgs e)
        {
            if (MainWindow.socket != null)
            {
                string msg = messageInput.Text;
                if (!String.IsNullOrEmpty(msg))
                {
                    buffer = Encoding.Default.GetBytes(msg);
                    MainWindow.socket.Send(buffer, 0, buffer.Length, 0);
                    messageInput.Text = "";
                }
            }
        }


    }
}
