using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private byte[] buffer;
        private int recv;

        public ChatWindow(int receiver)
        {
            this.recv = receiver;
            InitializeComponent();
            recvNumber.Text = $"{recv}";


        }

        private void Send_Msg(object sender, RoutedEventArgs e)
        {
            if (MainWindow.socket != null)
            {
                string msg = messageInput.Text;


                if (!String.IsNullOrEmpty(msg))
                {
                    Order msgToSend = new Order(0, MainWindow.userAcc.token, MainWindow.userAcc.accNumber, recv, msg, DateTime.Now);

                    Serializer serializer = new Serializer();
                    buffer = serializer.Serialize_Obj(msgToSend);

                    MainWindow.socket.Send(buffer, 0, buffer.Length, 0);

                    TextBox txtbox = new TextBox();
                    txtbox.Text = msgToSend.message;
                    txtbox.Style = MainWindow.msgStyle;
                    txtbox.HorizontalAlignment = HorizontalAlignment.Right;
                    msgStackPanel.Children.Add(txtbox);

                    messageInput.Text = "";
                }
            }
        }

    }
}
