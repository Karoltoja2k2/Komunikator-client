using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public ChatWindow chatWindow;

        public static Serializer serializer = new Serializer();
        public static Order deserializeOrderType = new Order();

        public static Style msgStyle = Application.Current.FindResource("messageBox") as Style;

        public MainWindow(Socket connetion, User connectedUser)
        {
            userAcc = connectedUser;
            socket = connetion;
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallBack, socket);
            InitializeComponent();
            accNumberBlock.Text = $"{connectedUser.accNumber}";

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
            Order order = (Order)serializer.Deserialize_Obj(recBuf, deserializeOrderType);
            if (chatWindow != null)
            {
                Dispatcher.Invoke(new Action(() => newMsg(order)));

            }

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallBack, socket);
        }

        private void newMsg(Order msgOrder)
        {
            TextBlock txtblock = new TextBlock();
            txtblock.Text = msgOrder.message;
            txtblock.Style = msgStyle;
            txtblock.HorizontalAlignment = HorizontalAlignment.Left;
            chatWindow.msgStackPanel.Children.Add(txtblock);
        }

        private void openConvButton(object sender, RoutedEventArgs e)
        {
            chatWindow = new ChatWindow();
            chatWindow.Owner = this;
            UiControl.OpenWindow(this, chatWindow);

        }

        private void logoutButton(object sender, RoutedEventArgs e)
        {
            socket.Close();
            LoginWindow window = new LoginWindow();
            UiControl.ChangeWindow(this, window);

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void searchUsersButton(object sender, RoutedEventArgs e)
        {
            // database interaction logic to find user matching with given number

            this.Hide();

            string[] accounts = File.ReadAllLines(@"C:\Users\Karol\Desktop\C#\Komunikator\Client\accounts.txt");
            List<string> foundAccounts = new List<string>();
            string searching = searchInput.Text;

            foreach(string account in accounts)
            {
                string[] acc = account.Split(';');
                if (acc[0] == searching)
                {
                    foundAccounts.Add(acc[0]);
                }
            }

            searchResults.Text = string.Join("\r\n", foundAccounts.ToArray());

        }
    }
}

