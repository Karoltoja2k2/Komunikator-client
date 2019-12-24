using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static Socket socket;
        public static User userAcc;
        public int BUFFER_SIZE = 2048;
        public byte[] buffer = new byte[2048];

        public static Serializer serializer = new Serializer();

        public MainWindow(Socket connetion, User connectedUser)
        {
            userAcc = connectedUser;
            socket = connetion;
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallBack, socket);
            InitializeComponent();
        }

        private void receiveCallBack(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received;
            try
            {
                received = socket.EndReceive(AR);
            }
            catch (SocketException) { return; }
            catch (ObjectDisposedException) { return; }
            

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            receiveBlockTest.Dispatcher.Invoke(new Action(() => receiveBlockTest.Text = text));
        
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallBack, socket);
        }

        private void changeText(string str)
        {
            receiveBlockTest.Text = str;
        }

        private void openConvButton(object sender, RoutedEventArgs e)
        {
            ChatWindow window = new ChatWindow();
            window.Owner = this;
            UiControl.OpenWindow(this, window);

        }

        private void logoutButton(object sender, RoutedEventArgs e)
        {
            socket.Close();
            LoginWindow window = new LoginWindow();
            UiControl.ChangeWindow(this, window);

        }

    }
}

