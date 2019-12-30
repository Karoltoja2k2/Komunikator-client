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
using Client.Pages;
using Client.Resources;

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

            foreach (Conversation conv in userAcc.conversations)
            {
                renderFriendListElem(conv.receiver);
            }
            // accNumberBlock.Text = $"{connectedUser.accNumber}";
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

            // Calls main thread to complete order
            Dispatcher.Invoke(new Action(() => manageIncomingOrder(order)));
           

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallBack, socket);
        }

        private void manageIncomingOrder(Order order)
        {
            if (order.orderType == 0)
            {
                if (chatWindow != null)
                {
                    foreach (Conversation conv in userAcc.conversations)
                    {
                        if (conv.you == order.receiver)
                        {
                            conv.messages.Add(order);
                            break;
                        }
                    }

                    TextBox txtbox = new TextBox();
                    txtbox.Text = order.message;
                    txtbox.Style = msgStyle;
                    txtbox.HorizontalAlignment = HorizontalAlignment.Left;
                    chatWindow.msgStackPanel.Children.Add(txtbox);
                }
            }
            else if (order.orderType == 1)
            {
                Button btn = new Button();
                btn.Content = $"{order.sender} sends friend request, click to acc";
                btn.CommandParameter = order.sender;
                btn.Click += AccFRequest;
                friendsStackPanel.Children.Add(btn);
            }
            else if (order.orderType == 2)
            {
                userAcc.conversations.Add(new Conversation(userAcc.accNumber, order.sender));

                renderFriendListElem(order.sender);
            }
        }

        public void AccFRequest(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            int receiver = (int)btn.CommandParameter;
            friendsStackPanel.Children.Remove(btn);
            Order order = new Order(2, userAcc.token, userAcc.accNumber, receiver, DateTime.Now);

            byte[] sendBuff = serializer.Serialize_Obj(order);

            socket.Send(sendBuff, 0, sendBuff.Length, 0);

            userAcc.conversations.Add(new Conversation(userAcc.accNumber, order.receiver));
            renderFriendListElem(order.receiver);
            
        }

        public void renderFriendListElem(int rcv)
        {
            friendListElem panel = new friendListElem();
            panel.number.Text = $"{rcv}";
            panel.nick.Text = "nick not implemented";
            panel.openConv.CommandParameter = rcv;
            panel.openConv.Click += openConvButton;
            friendsStackPanel.Children.Add(panel);
        }


        public void openConvButton(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int receiver = (int)btn.CommandParameter;
            Conversation openedConv;
            foreach (Conversation conv in userAcc.conversations)
            {
                if (conv.receiver == receiver)
                {
                    openedConv = conv;

                    chatWindow = new ChatWindow(openedConv);
                    // chatWindow.Owner = this;
                    UiControl.OpenWindow(this, chatWindow);
                    break;
                }
            }
        }



        private void searchContactWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SearchWindow searchWindow = new SearchWindow(userAcc);
            searchWindow.Owner = this;
            UiControl.OpenWindow(this, searchWindow);
        }

        private void logoutButton(object sender, RoutedEventArgs e)
        {
            socket.Close();
            LoginWindow window = new LoginWindow();
            UiControl.ChangeWindow(this, window);
        }

        private void mainOnExit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // this will close all chat windows in future, but now there is only one chat window open at time
            if (chatWindow != null)
            {
                chatWindow.Close();
            }
        }





        // STYLES FUNCTIONS

        private void searchButtonStyleFocus(object sender, MouseEventArgs e)
        {
            searchButton.Foreground = new SolidColorBrush(Colors.ForestGreen);
            searchButton.FontSize += 3;
        }

        private void searchButtonStyleFocusOut(object sender, MouseEventArgs e)
        {
            searchButton.Foreground = new SolidColorBrush(Colors.Black);
            searchButton.FontSize -= 3;
        }






    }
}

