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
using System.Windows.Shapes;

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static Socket socket;

        public MainWindow(Socket connetion)
        {
            socket = connetion;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChatWindow window = new ChatWindow();
            UiControl.OpenWindow(this, window);

        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            socket.Close();
            LoginWindow window = new LoginWindow();
            UiControl.ChangeWindow(this, window);

        }
    }
}

