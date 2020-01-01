using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using Client.Pages;

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LogPage1 page1;
        public LoadingPage loadingPage;
        public Socket socket;
        public IPEndPoint endPoint;

        public LoginWindow()
        {
            InitializeComponent();

            loadingPage = new LoadingPage();
            Loaded += loadLoadingPage;

        }


        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                socket.EndConnect(ar);
                if (socket.Connected)
                {
                    Dispatcher.Invoke((Action)delegate
                    {
                        page1 = new LogPage1(this, socket); // login page
                        LogFrame.NavigationService.Navigate(page1);
                    });
                }
                return;


            }
            catch (SocketException)
            {
                socket.BeginConnect(endPoint, ConnectCallBack, socket);
                return;
            }
        }


        private void loadLoadingPage(object sender, RoutedEventArgs e)
        {
            LogFrame.NavigationService.Navigate(loadingPage);


            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);

            socket.BeginConnect(endPoint, ConnectCallBack, socket);
        }

    }
}
