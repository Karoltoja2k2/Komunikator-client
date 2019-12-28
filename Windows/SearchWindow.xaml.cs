using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Logika interakcji dla klasy SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        byte[] buffer;

        public SearchWindow()
        {
            InitializeComponent();
        }

        // private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        // {
        //     Regex regex = new Regex("[^0-9]+");
        //     e.Handled = regex.IsMatch(e.Text);
        // }
        // 

        private void declineButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void showMainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Show();
        }

        private void searchButtonClick(object sender, RoutedEventArgs e)
        {
            // database interaction logic to find user matching with given number

            resultStackPanel.Children.Clear();
            string[] accounts = File.ReadAllLines(@"C:\Users\Karol\Desktop\C#\Komunikator\Client\accounts.txt");
            List<string> foundAccounts = new List<string>();
            string searching1 = search1.Text;
            string searching2 = search2.Text;

            foreach (string account in accounts)
            {
                string[] acc = account.Split(';');
                if (acc[0] == searching1 | acc[0] == searching2)
                {
                    foundAccounts.Add(acc[0]);
                }
            }

            if (foundAccounts.Count == 0)
            {
                resultCounter.Text = "Brak wyników";
            }
            else
            {
                resultCounter.Text = $"{foundAccounts.Count} wyników";
                foreach (string acc in foundAccounts)
                {
                    Button btn = new Button();
                    btn.Content = acc;
                    btn.CommandParameter = acc;
                    resultStackPanel.Children.Add(btn);
                    btn.Click += sendFriendRequest;
                }
            }
            
        }

        private void sendFriendRequest(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string receiver = (string)btn.CommandParameter;
            int recv = Int32.Parse(receiver);
            Order fRequest = new Order(1, MainWindow.userAcc.token, MainWindow.userAcc.accNumber, recv, DateTime.Now);

            Serializer serializer = new Serializer();
            buffer = serializer.Serialize_Obj(fRequest);

            MainWindow.socket.Send(buffer, 0, buffer.Length, 0);

            // send friend request
        }
    }
}
