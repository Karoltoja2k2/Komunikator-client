using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        public User userAcc;
        public Socket connection;

        public Serializer serializer = new Serializer();

        public ProfileWindow(User user, Socket connection)
        {
            userAcc = user;
            this.connection = connection;
            serializer = new Serializer();
            Loaded += renderData;
            InitializeComponent();
        }

        public void serverError()
        {
            LoginWindow loginWindow = new LoginWindow();
            UiControl.ChangeWindow(this, loginWindow);
        }

        private void declineButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void showMainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Show();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void renderData(object sender, RoutedEventArgs e)
        {
            emailBox.Text = userAcc.email;
            nickNameBox.Text = userAcc.nickName;
            phoneNumBox.Text = $"{userAcc.phoneNum}";
        }

        private void confirmChanges(object sender, RoutedEventArgs e)
        {
            string email = emailBox.Text;

            if (!email.Contains("@"))
            {
                emailBox.BorderBrush = Brushes.Red;
                return;
            }
            string nickName = nickNameBox.Text;
            int pNum;
            Int32.TryParse(phoneNumBox.Text, out pNum);

            if (email != userAcc.email || nickName != userAcc.nickName || pNum != userAcc.phoneNum)
            {
                Order changeOrder = new Order(9, userAcc.accNumber, email, nickName, pNum);
                byte[] sendBuff = serializer.Serialize_Obj(changeOrder);
                try
                {
                    MainWindow.socket.Send(sendBuff, 0, sendBuff.Length, 0);
                    this.Close();
                }
                catch (SocketException) { serverError(); return; }
            }
        }

    }
}
