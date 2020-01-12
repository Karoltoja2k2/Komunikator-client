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
using Client.Resources;

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private static readonly byte[] buffer = new byte[2048];
        User userAcc;
        Socket connection;
        Serializer serializer;

        public SearchWindow(User user, Socket connection)
        {
            userAcc = user;
            this.connection = connection;
            serializer = new Serializer();
            InitializeComponent();
            
        }

        private void declineButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void showMainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Show();
        }

        public void serverError()
        {
            LoginWindow loginWindow = new LoginWindow();
            UiControl.ChangeWindow(this, loginWindow);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void searchButtonClick(object sender, RoutedEventArgs e)
        {
            // database interaction logic to find user matching with given number

            resultStackPanel.Children.Clear();
            int accNum;
            int phoneNum;
            if (!String.IsNullOrEmpty(search1.Text))
                Int32.TryParse(search1.Text, out accNum);
            else
                accNum = 0;

            if (!String.IsNullOrEmpty(search2.Text))
                Int32.TryParse(search2.Text, out phoneNum);
            else
                phoneNum = 0;

            string nickName = search3.Text;
            string email = search4.Text;

            Order searchRequest = new Order(7, userAcc.accNumber, accNum, phoneNum, nickName, email);
            byte[] sendBuff = serializer.Serialize_Obj(searchRequest);
            MainWindow.socket.Send(sendBuff, 0, sendBuff.Length, 0);
        }

        public void searchResultsRender(Order receivedOrder)
        {
            List<User> foundAccounts = new List<User>();

            foreach (User profile in receivedOrder.foundProfiles)
            {
                if (profile.accNumber != userAcc.accNumber)
                {
                    if (!userAcc.friendList.Contains(profile.accNumber))
                    {
                        foundAccounts.Add(profile);
                    }
                }
            }

            if (foundAccounts.Count == 0)
            {
                // Dispatcher.Invoke(new Action(() => resultCounter.Text = "Brak wyników"));
                resultCounter.Text = "Brak wyników";
            }
            else
            {
                resultCounter.Text = $"Znaleziono {foundAccounts.Count} kontakt(ów)";
                foreach (User acc in foundAccounts)
                {
                    searchFriendElem elem = new searchFriendElem();
                    elem.addFriendButton.CommandParameter = acc.accNumber;
                    elem.addFriendButton.Click += sendFriendRequest;
                    elem.numberBox.Text = $"{acc.accNumber}";
                    elem.emailBox.Text = acc.email;
                    elem.nickBox.Text = acc.nickName;
                    if (acc.phoneNum != 0)
                        elem.phoneNumBox.Text = $"{acc.phoneNum}";

                    resultStackPanel.Children.Add(elem);

                }
            }

        }

        private void sendFriendRequest(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int receiver = (int)btn.CommandParameter;
            btn.IsEnabled = false;
            Order fRequest = new Order(1, MainWindow.profile.token, MainWindow.profile.accNumber, receiver, DateTime.Now);

            byte[] sendBuff = serializer.Serialize_Obj(fRequest);
            try
            {
                MainWindow.socket.Send(sendBuff, 0, sendBuff.Length, 0);
            }
            catch (SocketException) { serverError(); return; }

            // send friend request
        }


    }
}
