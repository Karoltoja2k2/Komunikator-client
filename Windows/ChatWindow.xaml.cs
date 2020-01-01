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
    /// Logika interakcji dla klasy ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private byte[] buffer;
        public Conversation conv;

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

        private void Send_Msg(object sender, RoutedEventArgs e)
        {
            if (MainWindow.socket != null)
            {
                string msg = messageInput.Text;


                if (!String.IsNullOrEmpty(msg))
                {
                    Order msgToSend = new Order(0, MainWindow.userAcc.token, conv.you, conv.receiver, msg, DateTime.Now);
                    conv.messages.Add(msgToSend);

                    Serializer serializer = new Serializer();
                    byte[] sendBuff = serializer.Serialize_Obj(msgToSend);

                    try
                    {
                        MainWindow.socket.Send(buffer, 0, buffer.Length, 0);
                    }
                    catch (SocketException) { serverError(); return; }
                    renderMessage(msgToSend);
                    messageInput.Text = "";
                }
            }
        }

        private void renderMessage(Order msg)
        {
            TextBox txtbox = new TextBox();
            txtbox.Text = msg.message;
            txtbox.Style = MainWindow.msgStyle;
            if (msg.sender == conv.you)
                txtbox.HorizontalAlignment = HorizontalAlignment.Right;
            else
                txtbox.HorizontalAlignment = HorizontalAlignment.Left;

            msgStackPanel.Children.Add(txtbox);
        }
    }
}
