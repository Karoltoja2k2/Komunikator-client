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
using Client.Resources;

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private byte[] buffer;
        public Conversation conv;
        public int prevMsgSnd;

        public ChatWindow(Conversation conv)
        {
            this.conv = conv;
            InitializeComponent();

            foreach (Order msg in conv.messages)
            {
                renderMessage(msg);
            }

            recvNumber.Text = $"{conv.receiver}";
        }

        public void serverError()
        {
            LoginWindow loginWindow = new LoginWindow();
            UiControl.ChangeWindow(this, loginWindow);
        }

        public void chatWinOnExit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.chatWindows.Remove(this);
        }

        private void Send_Msg(object sender, RoutedEventArgs e)
        {
            if (MainWindow.socket != null)
            {
                string msg = messageInput.Text;


                if (!String.IsNullOrEmpty(msg))
                {
                    Order msgToSend = new Order(0, MainWindow.profile.token, conv.you, conv.receiver, msg, DateTime.Now);
                    conv.messages.Add(msgToSend);

                    Serializer serializer = new Serializer();
                    byte[] sendBuff = serializer.Serialize_Obj(msgToSend);

                    try
                    {
                        MainWindow.socket.Send(sendBuff, 0, sendBuff.Length, 0);
                    }
                    catch (SocketException) { serverError(); return; }
                    renderMessage(msgToSend);
                    msgPanelScroll.ScrollToEnd();
                    messageInput.Text = "";
                }
            }
        }

        public void renderMessage(Order msg)
        {

            msgBoxRight msgBoxR;
            msgBoxLeft msgBoxL;
            Thickness margin;

            int imageHeight = 30;

            if (prevMsgSnd != 0 && msg.sender == prevMsgSnd)
            {
                margin = new Thickness(5, 0, 5, 2);
                imageHeight = 0;
            }
            else
            {
                margin = new Thickness(5, 8, 5, 2);
            }

            if (msg.sender == conv.receiver)
            {
                msgBoxL = new msgBoxLeft();
                msgBoxL.messageText.Text = msg.message;
                msgBoxL.Margin = margin;
                msgBoxL.profileImage.Height = imageHeight;
                msgStackPanel.Children.Add(msgBoxL);
            }
            else
            {
                msgBoxR = new msgBoxRight();
                msgBoxR.messageText.Text = msg.message;
                msgBoxR.Margin = margin;
                msgBoxR.profileImage.Height = imageHeight;
                msgStackPanel.Children.Add(msgBoxR);
            }
            prevMsgSnd = msg.sender;
        }

        // STYLES

        private void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            bool AutoScroll = true; ;
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (msgPanelScroll.VerticalOffset == msgPanelScroll.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    AutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    AutoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (AutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                msgPanelScroll.ScrollToVerticalOffset(msgPanelScroll.ExtentHeight);
            }
        }

    }
}
