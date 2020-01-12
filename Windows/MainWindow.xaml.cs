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
        public static User profile;
        public int BUFFER_SIZE = 2048;
        public byte[] buffer = new byte[2048];
        public static List<ChatWindow> chatWindows = new List<ChatWindow>();
        public SearchWindow searchWindow;
        public ProfileWindow profWindow;

        public static Serializer serializer = new Serializer();
        public static Order deserializeOrderType = new Order();

        public static List<friendRequestElem> reqPanelElements = new List<friendRequestElem>();
        public static Style msgStyle = Application.Current.FindResource("messageBox") as Style;


        public MainWindow(Socket connetion, User userProfile)
        {
            profile = userProfile;
            socket = connetion;
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallBack, socket);
            Loaded += showFriendList;
            InitializeComponent();

            renderData();
        }

        public void renderData()
        {
            friendsStackPanel.Children.Clear();
            foreach (Conversation conv in profile.conversations)
            {
                renderFriendListElem(conv.receiver);
            }

            friendRequestPanel.Children.Clear();
            foreach (Order ord in profile.pendingOrders)
            {
                renderFriendRequestElem(ord.sender);
            }

            numberBlock.Text = $"{profile.accNumber}, {profile.nickName}";
        }


        public void serverError()
        {
            LoginWindow loginWindow = new LoginWindow();
            UiControl.ChangeWindow(this, loginWindow);
            return;
        }


        private void receiveCallBack(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received;
            try
            {
                received = socket.EndReceive(AR);
            }
            catch (SocketException) { Dispatcher.Invoke(new Action(() => serverError())); return; }
            catch (ObjectDisposedException) { Dispatcher.Invoke(new Action(() => serverError())); return; }
            

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            Order order = (Order)serializer.Deserialize_Obj(recBuf, deserializeOrderType);
            // Calls main thread to complete order
            Dispatcher.Invoke(new Action(() => manageIncomingOrder(order)));

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, receiveCallBack, socket);
        }

        private void manageIncomingOrder(Order order)
        {
            // if received message
            if (order.orderType == 0)
            {
                // find conversation and save message
                foreach (Conversation conv in profile.conversations)
                {
                    if (conv.you == order.receiver)
                    {
                        conv.messages.Add(order);
                        break;
                    }
                }

                foreach (ChatWindow chatWin in chatWindows)
                {
                    if (chatWin.conv.receiver == order.sender)
                        chatWin.renderMessage(order);
                }
            }

            // render
            else if (order.orderType == 1)
            {
                profile.pendingOrders.Add(order);
                renderFriendRequestElem(order.sender);
            }
            else if (order.orderType == 2)
            {
                profile.conversations.Add(new Conversation(profile.accNumber, order.sender));

                renderFriendListElem(order.sender);
            }
            else if (order.orderType == 8 && searchWindow != null)
            {
                searchWindow.searchResultsRender(order);
            }
        }

        public void AccFRequest(object sender, RoutedEventArgs e)
        {

            Button btn = (Button)sender;
            int receiver = (int)btn.CommandParameter;
            Order order = new Order(2, profile.token, profile.accNumber, receiver, DateTime.Now);

            byte[] sendBuff = serializer.Serialize_Obj(order);
            try
            {
                socket.Send(sendBuff, 0, sendBuff.Length, 0);
            }
            catch (SocketException) { serverError(); return; }

            profile.conversations.Add(new Conversation(profile.accNumber, order.receiver));

            var acceptedFReq = profile.pendingOrders.Find(req => req.sender == receiver);
            profile.pendingOrders.Remove(acceptedFReq);

            renderData();
            
        }

        public void renderFriendListElem(int rcv)
        {
            friendListElem panel = new friendListElem();
            panel.number.Text = $"{rcv}";
            panel.nick.Text = "nick";
            panel.openConv.CommandParameter = rcv;
            panel.openConv.Click += openConvButton;
            friendsStackPanel.Children.Add(panel);
        }

        public void renderFriendRequestElem(int rcv)
        {
            friendRequestElem panel = new friendRequestElem();
            panel.number.Text = $"{rcv}";
            panel.nick.Text = $"nick";
            panel.acceptReq.CommandParameter = rcv;
            panel.acceptReq.Click += AccFRequest;
            friendRequestPanel.Children.Add(panel);
        }



        public void openConvButton(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int receiver = (int)btn.CommandParameter;
            Conversation openedConv;
            foreach (Conversation conv in profile.conversations)
            {
                if (conv.receiver == receiver)
                {
                    foreach (ChatWindow chatWin in chatWindows)
                    {
                        if (chatWin.conv.receiver == receiver)
                            return;
                    }
                    openedConv = conv;

                    ChatWindow chatWindow = new ChatWindow(openedConv);
                    chatWindows.Add(chatWindow);
                    UiControl.OpenWindow(this, chatWindow);
                    break;
                }
            }
        }


        private void profileDataButton(object sender, RoutedEventArgs e)
        {
            this.Hide();
            profWindow = new ProfileWindow(profile, socket);
            profWindow.Owner = this;
            UiControl.OpenWindow(this, profWindow);
        }

        private void searchContactWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
            searchWindow = new SearchWindow(profile, socket);
            searchWindow.Owner = this;
            UiControl.OpenWindow(this, searchWindow);
        }

        private void logoutButton(object sender, RoutedEventArgs e)
        {
            socket.Close();
        }

        private void mainOnExit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int openedWindows = chatWindows.Count;
            for (int i = 0; i < openedWindows; i++)
            {
                MainWindow.chatWindows.ElementAt<ChatWindow>(0).Close();
            }
        }

        // STYLES

        private void showFRequests(object sender, RoutedEventArgs e)
        {
            int reqCount = profile.pendingOrders.Count;
            if (reqCount != 0)
            {
                if (friendRequestPanel.Height == 0)
                {
                    resultsCounter.Visibility = Visibility.Visible;
                    resultCountBox.Text = $"Zaproszenia: {reqCount}";

                    friendRequestPanel.Height = Double.NaN;
                    friendsStackPanel.Height = 0;
                }
                else
                {
                    resultsCounter.Visibility = Visibility.Collapsed;
                    friendRequestPanel.Height = 0;
                }
            }
            else
            {
                friendsStackPanel.Height = 0;
                resultsCounter.Visibility = Visibility.Visible;
                resultCountBox.Text = $"Brak zaproszeń";
            }
        }

        private void showFriendList(object sender, RoutedEventArgs e)
        {
            int frCount = profile.conversations.Count;
            if (frCount != 0)
            {
                if (friendsStackPanel.Height == 0)
                {
                    resultsCounter.Visibility = Visibility.Visible;
                    resultCountBox.Text = $"Kontakty: {frCount}";

                    friendsStackPanel.Height = Double.NaN;
                    friendRequestPanel.Height = 0;
                }
                else
                {
                    resultsCounter.Visibility = Visibility.Collapsed;
                    friendsStackPanel.Height = 0;
                }
            }
            else
            {
                friendRequestPanel.Height = 0;
                resultsCounter.Visibility = Visibility.Visible;
                resultCountBox.Text = $"Brak kontaktów";
            }
        }


    }
}

